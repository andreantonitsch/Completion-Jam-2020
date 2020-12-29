using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public AudioSource audio;

    public void Mute()
    {
        audio.mute = !audio.mute;
    }

    public void LoadIntroScene()
    {
        SceneManager.LoadScene("Intro");
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }

}
