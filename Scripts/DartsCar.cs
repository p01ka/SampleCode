using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DartsCar : MonoBehaviour
{
    [SerializeField] MoveCamera moveCamera;
    [SerializeField] Transform carList;
    [SerializeField] Transform pnevmaticPlatform;
    [SerializeField] Transform pointCarInPnevmatic;
    [SerializeField] ScoreColider scoreColider;
    [SerializeField] GameObject explosionEffect;

    [SerializeField] float maxCellSide, maxCellUp;
    [SerializeField] float sensitivityMouse;

    [SerializeField] float energyIndicator;
    [SerializeField] int minEnergy, maxEnergy;

    GameObject carPrefab;
    FlyCar currentFlyCar;

    Coroutine energyBotCoroutine;
    Coroutine finishRollCorouutine;

    Vector3 defaultPosPnevmo;
    Vector3 defaultPosRotate;

    Vector3 startPositionMouse;
    Vector3 lastPositionCell;
    Vector3 screenPoint;
    Vector3 curScreenPoint;
    Vector3 curPositionMouse;

    int countEnergy;
    int countShoot = 5;
    int countScrore;

    bool isFinishAim;
    bool isStartAimBot;
    bool isShootCar;
    bool isFlyCar;
    bool isDestroyGun;

    private void Start()
    {
        defaultPosPnevmo = pnevmaticPlatform.localPosition;
        defaultPosRotate = pnevmaticPlatform.parent.localEulerAngles;
        CreateCar();
    }
    private void Update()
    {
        if (!GameController.Instance.IsThrowsOver())
        {
            if (!isFinishAim)
            {
                if (GameController.Instance.isBotRound)

                {
                    if (!isStartAimBot)
                        AimBot();
                }
                else
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
                        curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
                        startPositionMouse = Camera.main.ScreenToWorldPoint(curScreenPoint);
                        lastPositionCell = transform.position + transform.forward;
                        transform.LookAt(lastPositionCell);
                    }
                    if (Input.GetMouseButton(0))
                    {
                        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
                        curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
                        curPositionMouse = Camera.main.ScreenToWorldPoint(curScreenPoint);
                        Vector3 target = curPositionMouse - startPositionMouse;
                        float x = target.x * sensitivityMouse;
                        float y = target.y * sensitivityMouse;
                        transform.rotation = Quaternion.Euler(new Vector3
                        {
                            x = Mathf.Clamp(transform.localRotation.x - y, -maxCellUp, 0),
                            y = Mathf.Clamp(transform.localRotation.y + x, -maxCellSide, maxCellSide),
                            z = 0
                        });
                    }
                    if (Input.GetMouseButtonUp(0))
                    {
                        FinishAim();
                    }
                }
            }
            else
            {
                if (!isShootCar)
                {
                    if (isDestroyGun)
                    {
                        if (finishRollCorouutine == null)
                        {
                            finishRollCorouutine = StartCoroutine(FinishRoll());
                        }
                        return;
                    }
                    if (GameController.Instance.isBotRound)
                    {
                        if (energyBotCoroutine == null)
                        {
                            energyBotCoroutine = StartCoroutine(EnergyBotCoroutine());
                        }
                    }
                    else
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            UIController.Instance.SetActivateEnergy(true);
                        }
                        if (Input.GetMouseButton(0))
                        {
                            isDestroyGun = UIController.Instance.AddIsMaxEnergy(energyIndicator);
                        }
                        if (Input.GetMouseButtonUp(0))
                        {
                            ShootCar();
                        }
                    }
                }
                else
                {
                    if (isFlyCar && currentFlyCar.IsEndFly())
                    {
                        if (finishRollCorouutine == null)
                        {
                            finishRollCorouutine = StartCoroutine(FinishRoll());
                        }
                    }

                }
            }
        }
        else
        {
            GameController.Instance.EndGame();
            this.enabled = false;
        }
    }
    void FinishAim()
    {
        isFinishAim = true;
        UIController.Instance.SetActivateEnergy(true);
    }
    void ShootCar()
    {
        isShootCar = true;
        StartCoroutine(ForceImpulse());
    }
    IEnumerator ForceImpulse()
    {
        UIController.Instance.SetActivateEnergy(false);
        moveCamera.SetCar(currentFlyCar.transform);
        pnevmaticPlatform.DOLocalMoveZ(0, 0.075f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.075f);
        currentFlyCar.AddForceCar(Mathf.Lerp(minEnergy, maxEnergy, UIController.Instance.GetEnergyCount()));
        yield return new WaitForSeconds(1f);
        isFlyCar = true;
    }
    void AimBot()
    {
        isStartAimBot = true;
        int count = Random.Range(1, 4);
        Sequence rotateAnim = DOTween.Sequence();
        for (int i = 0; i < count; i++)
        {
            Vector3 target = new Vector3
            {
                x = Random.Range(-maxCellUp, 0),
                y = Random.Range(-maxCellSide / 3, maxCellSide / 3),
                z = 0
            };
            if (i < count - 1)
                rotateAnim.Append(transform.DORotate(target, Random.Range(3, 5))).SetEase(Ease.Linear);
            else
            {
                rotateAnim.Append(transform.DORotate(target, Random.Range(3, 5))).OnComplete(FinishAim);
            }
        }
    }

    IEnumerator EnergyBotCoroutine()
    {
        UIController.Instance.SetActivateEnergy(true);
        float botEnergy = Random.Range(0.5f, 0.85f);
        while (UIController.Instance.GetEnergyCount() < botEnergy)
        {
            yield return new WaitForEndOfFrame();
            isDestroyGun = UIController.Instance.AddIsMaxEnergy(energyIndicator);
            if (isDestroyGun)
            {
                StopCoroutine(energyBotCoroutine);
            }
        }
        ShootCar();
    }
    void CreateCar()
    {
        // int indexCar = 0;
        int indexCar = Random.Range(0, carList.childCount);
        GameObject obj = Instantiate(carList.GetChild(indexCar).gameObject);
        obj.transform.SetParent(pnevmaticPlatform);
        obj.transform.localPosition = pointCarInPnevmatic.localPosition;
        obj.SetActive(true);
        currentFlyCar = obj.transform.GetComponent<FlyCar>();
    }
    void RestartCar()
    {
        if (currentFlyCar != null)
        {
            currentFlyCar.transform.GetComponent<Deform>().DiactiveDeform();
        }
        pnevmaticPlatform.localPosition = defaultPosPnevmo;
        pnevmaticPlatform.parent.localEulerAngles = defaultPosRotate;
        moveCamera.MoveStartPosition();
        CreateCar();
    }
    IEnumerator FinishRoll()
    {
        if (GameController.Instance.isBotRound)
        {
            isStartAimBot = false;
            energyBotCoroutine = null;
        }
        moveCamera.namePlayer.DiactiveName();
        if (!isDestroyGun)
        {
            scoreColider.Activate(currentFlyCar.transform.position);
            yield return new WaitForFixedUpdate();
            GameController.Instance.AddCountScore(scoreColider.scoreRound);
            scoreColider.Diactive();
        }
        else
        {

            ExplosionActivate();
            Destroy(currentFlyCar.gameObject);
            UIController.Instance.SetActivateEnergy(false);
            GameController.Instance.AddCountScore(0);
            yield return new WaitForSeconds(1);
        }
        RestartCar();
        RestartSetting();
    }
    public void RestartSetting()
    {
        isFlyCar = false;
        isFinishAim = false;
        isShootCar = false;
        isDestroyGun = false;
        finishRollCorouutine = null;
    }
    public void RestartButton()
    {
        SceneManager.LoadScene(0);
    }
    void ExplosionActivate()
    {
        GameObject obj = Instantiate(explosionEffect);
        obj.transform.position = explosionEffect.transform.position;
        obj.SetActive(true);
        //  Destroy(obj, 5);
    }
}
