using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    public static InputManager instance;


    public bool press1 = false;
    public bool press2 = false;
    public bool press3 = false;

    public bool pressW = false;
    public bool pressS = false;
    public bool pressA = false;
    public bool pressD = false;
    public bool pressSpaceBar = false;
    public bool pressE = false;


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


        #region InputNumbers


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            press1 = true;
        }
        else if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            press1 = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            press2 = true;
        }
        else if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            press2 = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            press3 = true;
        }
        else if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            press3 = false;
        }

        #endregion

        if (Input.GetKeyDown(KeyCode.Space))
        {
            pressSpaceBar = true;
        }

        #region InputLetters

        if (Input.GetKeyDown(KeyCode.E))
        {
            pressE = true;
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            pressE = false;
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
