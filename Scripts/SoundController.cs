using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Список звуков в массиве
 * 0.Победа1, 1. Победа2, 2.Проигрышь
 */
public class SoundController : MonoBehaviour
{
    public AudioSource[] sourceInGame;
    public bool isActiveSound;


    public static SoundController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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
    public void PlaySound(AudioSource source)
    {
        if (!isActiveSound) { return; }
        source.Play();
    }
    public void WinSound()
    {
        PlaySound(sourceInGame[0]);
        PlaySound(sourceInGame[1]);
    }
    public void LoseSound()
    {
        PlaySound(sourceInGame[2]);
    }
}
