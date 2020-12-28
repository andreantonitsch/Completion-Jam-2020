using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    public static InputManager instance;

    [HideInInspector]
    public bool pressW = false;
    [HideInInspector]
    public bool pressS = false;
    [HideInInspector]
    public bool pressA = false;
    [HideInInspector]
    public bool pressD = false;
    [HideInInspector]
    public bool pressSpaceBar = false;
    [HideInInspector]
    public bool pressE;


    // Start is called before the first frame update
    void Start()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

        #region Input

        if (Input.GetKeyDown(KeyCode.E))
        {
            pressE = true;
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            pressE = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            pressSpaceBar = true;
        }


        if (Input.GetKeyDown("w"))
        {
            pressW = true;
        }
        else if (Input.GetKeyUp("w"))
        {
            pressW = false;
        }

        if (Input.GetKeyDown("s"))
        {
            pressS = true;
        }
        else if (Input.GetKeyUp("s"))
        {
            pressS = false;
        }

        if (Input.GetKeyDown("a"))
        {
            pressA = true;
        }
        else if (Input.GetKeyUp("a"))
        {
            pressA = false;
        }

        if (Input.GetKeyDown("d"))
        {
            pressD = true;
        }
        else if (Input.GetKeyUp("d"))
        {
            pressD = false;
        }

        #endregion



    }
}