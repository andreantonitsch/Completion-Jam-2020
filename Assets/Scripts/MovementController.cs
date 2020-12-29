using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{

    public static MovementController instance;
    // Start is called before the first frame update
    private Rigidbody rig;
    public float forceMult;
    public float jumpMult;

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

    void FixedUpdate()
    {


        #region AddForce


        if (InputManager.instance.pressSpaceBar)
        {
            //Physics.Raycast(player.)
            PlayerController.instance.PlayerRig.AddForce(transform.up * jumpMult, ForceMode.Impulse);

            InputManager.instance.pressSpaceBar = false;
        }

        if (InputManager.instance.pressW)
        {

            if (InputManager.instance.pressA)
            {
                PlayerController.instance.PlayerRig.AddTorque(Camera.main.transform.right * forceMult);
                PlayerController.instance.PlayerRig.AddForce(Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)) * forceMult * .75f);

                PlayerController.instance.PlayerRig.AddTorque(Camera.main.transform.forward * forceMult);
                PlayerController.instance.PlayerRig.AddForce(Vector3.Scale(-Camera.main.transform.right, new Vector3(1, 0, 1)) * forceMult * 0.25f);
            }
            else
            if (InputManager.instance.pressD)
            {
                PlayerController.instance.PlayerRig.AddTorque(Camera.main.transform.right * forceMult);
                PlayerController.instance.PlayerRig.AddForce(Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)) * forceMult * .75f);

                PlayerController.instance.PlayerRig.AddTorque(-Camera.main.transform.forward * forceMult);
                PlayerController.instance.PlayerRig.AddForce(Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1)) * forceMult * 0.25f);
            }
            else
            {
                PlayerController.instance.PlayerRig.AddTorque(Camera.main.transform.right * forceMult);
                PlayerController.instance.PlayerRig.AddForce(Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)) * forceMult);
            }
            return;
        }

        if (InputManager.instance.pressS)
        {
            if (InputManager.instance.pressA)
            {
                PlayerController.instance.PlayerRig.AddTorque(-Camera.main.transform.right * forceMult);
                PlayerController.instance.PlayerRig.AddForce(Vector3.Scale(-Camera.main.transform.forward, new Vector3(1, 0, 1)) * forceMult * .75f);

                PlayerController.instance.PlayerRig.AddTorque(Camera.main.transform.forward * forceMult);
                PlayerController.instance.PlayerRig.AddForce(Vector3.Scale(-Camera.main.transform.right, new Vector3(1, 0, 1)) * forceMult * 0.25f);
            }
            else
            if (InputManager.instance.pressD)
            {
                PlayerController.instance.PlayerRig.AddTorque(-Camera.main.transform.right * forceMult);
                PlayerController.instance.PlayerRig.AddForce(Vector3.Scale(-Camera.main.transform.forward, new Vector3(1, 0, 1)) * forceMult * .75f);

                PlayerController.instance.PlayerRig.AddTorque(-Camera.main.transform.forward * forceMult);
                PlayerController.instance.PlayerRig.AddForce(Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1)) * forceMult * 0.25f);
            }
            else
            {
                PlayerController.instance.PlayerRig.AddTorque(Camera.main.transform.right * forceMult);
                PlayerController.instance.PlayerRig.AddForce(Vector3.Scale(-Camera.main.transform.forward, new Vector3(1, 0, 1)) * forceMult);
            }

            return;

        }

        if (InputManager.instance.pressA)
        {
            PlayerController.instance.PlayerRig.AddTorque(Camera.main.transform.forward * forceMult);
            PlayerController.instance.PlayerRig.AddForce(Vector3.Scale(-Camera.main.transform.right, new Vector3(1, 0, 1)) * forceMult);

        }

        if (InputManager.instance.pressD)
        {
            PlayerController.instance.PlayerRig.AddTorque(-Camera.main.transform.forward * forceMult);
            PlayerController.instance.PlayerRig.AddForce(Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1)) * forceMult);

        }

      


        #endregion
    }
}
