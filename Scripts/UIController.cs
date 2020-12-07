using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject lostPanel;
    [SerializeField] Image energyImage;
    [SerializeField] Text countScorePlayerText;
    [SerializeField] Text countScoreBotText;
    [SerializeField] Text countShootText;



    public static UIController Instance;




    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    //EnergyIndicator
    public void SetActivateEnergy(bool isActive)
    {
        if (isActive) { energyImage.fillAmount = 0; }
        energyImage.transform.parent.gameObject.SetActive(isActive);
    }
    public bool AddIsMaxEnergy(float addEnergy)
    {
        energyImage.fillAmount += addEnergy * Time.deltaTime;
        return energyImage.fillAmount >= 1;
    }

    public float GetEnergyCount()
    {
        return energyImage.fillAmount;
    }

    //Text
    public void SetCountScore(int score, bool isBot)
    {
        if (isBot)
        {
            countScoreBotText.text = score.ToString();
        }
        else
        {
            countScorePlayerText.text = score.ToString();
        }
    }
    public void SetCountShoot(int shoot)
    {
        countShootText.text = shoot.ToString();
    }
    //
    public void EndGame(bool isWin)
    {
        if (isWin)
        {
            winPanel.SetActive(true);
        }
        else
        {
            lostPanel.SetActive(true);
        }
    }
    //Button
    public void RestartButton()
    {
        SceneManager.LoadScene(0);
    }
}
