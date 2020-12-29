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
    public float v_axis = 0.0f;
    public float h_axis = 0.0f;
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

        if (Input.GetButtonDown("Jump"))
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



        v_axis = Input.GetAxis("Vertical");
        h_axis = Input.GetAxis("Horizontal");
        if (v_axis > 0.1f)
        {
            pressW = true;
        }
        else if (v_axis <= 0.1f)
        {
            pressW = false;
        }

        if (v_axis < -0.1f)
        {
            pressS = true;
        }
        else if (v_axis >= -0.1f)
        {
            pressS = false;
        }

        if (h_axis < -0.1f)
        {
            pressA = true;
        }
        else if (h_axis >= -0.1f)
        {
            pressA = false;
        }

        if (h_axis > 0.1f)
        {
            pressD = true;
        }
        else if (h_axis <= 0.1f)
        {
            pressD = false;
        }

        #endregion



    }
}
