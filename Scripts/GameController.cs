using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] NamePlayerOnCar namePlayer;

    public int countScoreInPlayer = 0;
    public int countScoreInBot = 0;
    public int countShootInPlayer = 5;
    public int countShootInBot = 5;
    public bool isBotRound;

    bool isThrowsOver;

    public static GameController Instance;

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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddCountScore(int scoreRound)
    {
        if (isBotRound)
        {
            isBotRound = false;
            if (countShootInBot > 0)
                countShootInBot--;
            countScoreInBot += scoreRound;
            UIController.Instance.SetCountScore(countScoreInBot, true);
        }
        else
        {
            isBotRound = true;
            if (countShootInPlayer > 0)
            {
                countShootInPlayer--;
                UIController.Instance.SetCountShoot(countShootInPlayer);
            }

            countScoreInPlayer += scoreRound;
            UIController.Instance.SetCountScore(countScoreInPlayer, false);
        }
    }
    public bool IsThrowsOver()
    {
        if (countShootInBot == 0 && countShootInPlayer == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void EndGame()
    {
        UIController.Instance.EndGame(countScoreInPlayer >= countScoreInBot);
    }
}
