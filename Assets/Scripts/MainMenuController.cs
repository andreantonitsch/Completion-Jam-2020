using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Sprite sprite_audio_on;
    public Sprite sprite_audio_off;
    public Image audio_img;
    public AudioSource audio;

    // Dirty hack
    public void Mute()
    {
        audio.mute = !audio.mute;
        if (audio.mute)
        {
            audio_img.sprite = sprite_audio_off;
        }
        else
        {
            audio_img.sprite = sprite_audio_on;
        }
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
