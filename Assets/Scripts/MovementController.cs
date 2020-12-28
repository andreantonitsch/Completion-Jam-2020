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

        if (InputManager.instance.pressW)
        {
            PlayerController.instance.PlayerRig.AddTorque(Camera.main.transform.right * forceMult);
            PlayerController.instance.PlayerRig.AddForce(Vector3.Scale(Camera.main.transform.forward, new Vector3(1,0,1)) * forceMult);
        }

        if (InputManager.instance.pressS)
        {
            PlayerController.instance.PlayerRig.AddTorque(-Camera.main.transform.right * forceMult);
            PlayerController.instance.PlayerRig.AddForce(Vector3.Scale(-Camera.main.transform.forward, new Vector3(1, 0, 1)) * forceMult);

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

        if (InputManager.instance.pressSpaceBar)
        {
            //Physics.Raycast(player.)
            PlayerController.instance.PlayerRig.AddForce(transform.up * jumpMult, ForceMode.Impulse);

            InputManager.instance.pressSpaceBar = false;
        }
      

        #endregion
    }
}
