using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OutroController : MonoBehaviour
{
    public void LoadOutroScene()
    {
        SceneManager.LoadScene("Outro");
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            LoadOutroScene();
    }
}
