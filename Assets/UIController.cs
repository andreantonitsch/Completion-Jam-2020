using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject SphereUI;
    public GameObject CubeUI;
    public GameObject PyramidUI;

    void Start()
    {
        if (PlayerController.instance.currentShape == 0)
        {
            SphereUI.SetActive(true);
            CubeUI.SetActive(false);
            PyramidUI.SetActive(false);
        }
        else if (PlayerController.instance.currentShape == 1)
        {
            SphereUI.SetActive(false);
            CubeUI.SetActive(true);
            PyramidUI.SetActive(false);
        }
        else if (PlayerController.instance.currentShape == 2)
        {
            SphereUI.SetActive(false);
            CubeUI.SetActive(true);
            PyramidUI.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.instance.morphedShape)
        {
            if(PlayerController.instance.currentShape == 0)
            {
                SphereUI.SetActive(true);
                CubeUI.SetActive(false);
                PyramidUI.SetActive(false);
            }
            else if(PlayerController.instance.currentShape == 1)
            {
                SphereUI.SetActive(false);
                CubeUI.SetActive(true);
                PyramidUI.SetActive(false);
            }
            else if (PlayerController.instance.currentShape == 2)
            {
                SphereUI.SetActive(false);
                CubeUI.SetActive(true);
                PyramidUI.SetActive(false);
            }
        }
    }
}
