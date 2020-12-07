using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreColider : MonoBehaviour
{
    public int scoreRound = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Activate(Vector3 carPosition)
    {
        transform.position = carPosition;
        gameObject.SetActive(true);
    }
    public void Diactive()
    {
        gameObject.SetActive(false);
        scoreRound = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "10Score":
                if (scoreRound < 10)
                    scoreRound = 10;
                break;
            case "25Score":
                if (scoreRound < 25)
                    scoreRound = 25;
                break;
            case "100Score":
                if (scoreRound < 100)
                    scoreRound = 100;
                break;
        }
    }
}
