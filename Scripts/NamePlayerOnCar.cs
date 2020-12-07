using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NamePlayerOnCar : MonoBehaviour
{
    [SerializeField] Transform camera;
    [SerializeField] Text playerName;
    [SerializeField] string[] botsName;

    string nameBot;

    Vector3 defaultPosition;
    // Start is called before the first frame update
    void Start()
    {
        defaultPosition = transform.position;
        transform.LookAt(camera);
        nameBot = botsName[Random.Range(0, botsName.Length)];
        SetName();
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        transform.LookAt(camera);
    }

    public void DiactiveName()
    {
        playerName.gameObject.SetActive(false);
    }
    public void ActivateName()
    {
        SetName();
    }
    void SetName()
    {
        if (GameController.Instance.isBotRound)
        {
            playerName.text = nameBot;
            playerName.gameObject.SetActive(true);
        }
        else
        {
            playerName.text = "";
        }
    }

}
