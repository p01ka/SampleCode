using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectionCar : MonoBehaviour
{
    [SerializeField] Camera camera;
    [SerializeField] DartsCar dartsCar;
    [SerializeField] GameObject startButton;
    [SerializeField] Transform stendCar;
    [SerializeField] float distanceSwap;
    [SerializeField] float timeAnimationSwap;

    PresentationCur[] presentationCurs;

    public int numberCurInStend = 0;
    int countCar;

    float startPositionMouse;
    float currentPositionMouse;
    float directionX;

    // Start is called before the first frame update
    void Start()
    {
        countCar = stendCar.childCount;
        stendCar.GetChild(numberCurInStend).GetComponent<PresentationCur>().RotateAnim();
        presentationCurs = new PresentationCur[countCar];
        for (int i = 0; i < countCar; i++)
        {
            presentationCurs[i] = stendCar.GetChild(i).GetComponent<PresentationCur>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPositionMouse = Input.mousePosition.x;
            currentPositionMouse = Input.mousePosition.x;
        }
        if (Input.GetMouseButton(0))
        {
            currentPositionMouse = Input.mousePosition.x;
            directionX = currentPositionMouse - startPositionMouse;

            if (Mathf.Abs(directionX) > distanceSwap)
            {
                startPositionMouse = currentPositionMouse;

                if (directionX >= 0)
                {

                    if (numberCurInStend < countCar - 1)
                    {
                        numberCurInStend++;
                        DOTween.Kill(stendCar);
                        stendCar.DOLocalMoveX(-stendCar.GetChild(numberCurInStend).localPosition.x, timeAnimationSwap).OnComplete(StartRootAnim);
                    }
                }
                else
                {
                    if (numberCurInStend > 0)
                    {
                        numberCurInStend--;
                        DOTween.Kill(stendCar);
                        stendCar.DOLocalMoveX(-stendCar.GetChild(numberCurInStend).localPosition.x, timeAnimationSwap).OnComplete(StartRootAnim);
                    }
                }

            }
        }
    }
    public void StartRootAnim()
    {
        stendCar.GetChild(numberCurInStend).GetComponent<PresentationCur>().RotateAnim();
        for (int i = 0; i < countCar; i++)
        {
            if (i != numberCurInStend)
            {
                presentationCurs[i].StopRotate();
            }
        }
    }
    //public void StartButton()
    //{
    //    dartsCar.enabled = true;
    //    dartsCar.StartGunGame(stendCar.GetChild(numberCurInStend).gameObject);
    //    startButton.SetActive(false);
    //    camera.gameObject.SetActive(false);
    //    for (int i = 0; i < countCar; i++)
    //    {
    //        presentationCurs[i].StopRotate();
    //    }
    //}
   
}
