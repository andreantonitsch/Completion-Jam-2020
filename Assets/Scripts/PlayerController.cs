using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SHAPE { SPHERE, CUBE, PYRAMID }
public class PlayerController : MonoBehaviour
{

   

    public static PlayerController instance;

    public List<GameObject> shapeList;
    public int currentShape;

    public Cinemachine.CinemachineVirtualCamera virtCamera;
    public GameObject Player;
    public Rigidbody PlayerRig;

    public bool morphedShape { get; private set; }



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

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Player = shapeList[currentShape];
        PlayerRig = Player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!morphedShape && (InputManager.instance.press1 || InputManager.instance.press2 || InputManager.instance.press3))
        {
            Vector3 lastPos = Player.transform.position;
            Vector3 lastVel = Player.transform.TransformDirection(PlayerRig.velocity);
            Vector3 lastAngVel = Player.transform.TransformDirection(PlayerRig.angularVelocity);
            Quaternion lastRot = Player.transform.rotation;

            if (InputManager.instance.press1)
            {
                shapeList[currentShape].SetActive(false);
                currentShape = (int) SHAPE.SPHERE;

                MovementController.instance.forceMult = 50;

            }

            if (InputManager.instance.press2 )
            {
                shapeList[currentShape].SetActive(false);
                currentShape = (int)SHAPE.CUBE;

                if (shapeList[currentShape])
                {
                    MovementController.instance.forceMult = 35;
                   
                }
                else
                {
                    currentShape = (int)SHAPE.SPHERE;
                }

             

            }

            if (InputManager.instance.press3)
            {
                shapeList[currentShape].SetActive(false);
                currentShape = (int)SHAPE.PYRAMID;
                if (shapeList[currentShape])
                {
                    MovementController.instance.forceMult = 35;
                    
                }
                else
                {
                    currentShape = (int)SHAPE.SPHERE;
                }

                

            }

            UpdateReferences(lastPos, lastVel, lastAngVel, lastRot);

        }


        #region DeprecatedChangeShape

        //if (InputManager.instance.pressE && !morphedShape)
        //{
        //    Vector3 lastVel = Player.transform.TransformDirection(PlayerRig.velocity);
        //    Vector3 lastAngVel = Player.transform.TransformDirection(PlayerRig.angularVelocity);

        //    Quaternion lastRot = Player.transform.rotation;

        //    if (currentShape < shapeList.Count - 1)
        //    {
        //        shapeList[currentShape].SetActive(false);

        //        currentShape++;
        //        shapeList[currentShape].SetActive(true);
        //        shapeList[currentShape].transform.position = shapeList[currentShape-1].transform.transform.position;

        //    }
        //    else
        //    {
        //        shapeList[currentShape].SetActive(false);
        //        currentShape = 0;
        //        shapeList[currentShape].SetActive(true);
        //        shapeList[currentShape].transform.position = shapeList[shapeList.Count - 1].transform.transform.position;
        //    }


        //    if(currentShape == (int)SHAPE.SPHERE)
        //    {
        //        MovementController.instance.forceMult = 50;   
        //    }

        //    if (currentShape == (int)SHAPE.CUBE)
        //    {
        //        MovementController.instance.forceMult = 35;
        //    }

        //    Player = shapeList[currentShape];
        //    Player.transform.rotation = lastRot;
        //    PlayerRig = Player.GetComponent<Rigidbody>();
        //    PlayerRig.velocity = Player.transform.InverseTransformDirection(lastVel);
        //    PlayerRig.angularVelocity = Player.transform.InverseTransformDirection(lastAngVel);
        //    virtCamera.Follow = Player.transform;

        //    morphedShape = true;

        //    StartCoroutine(ChangeShapeDelay(0.2f));

        //}

        #endregion

    }


    private void UpdateReferences(Vector3 lastPos, Vector3 lastVel, Vector3 lastAngVel, Quaternion lastRot )
    {
        Player = shapeList[currentShape];
        shapeList[currentShape].SetActive(true);
        Player.transform.rotation = lastRot;
        Player.transform.position = lastPos;
        PlayerRig = Player.GetComponent<Rigidbody>();
        PlayerRig.velocity = Player.transform.InverseTransformDirection(lastVel);
        PlayerRig.angularVelocity = Player.transform.InverseTransformDirection(lastAngVel);
        virtCamera.Follow = Player.transform;

        morphedShape = true;

        StartCoroutine(ChangeShapeDelay(0.2f));
    }

    IEnumerator ChangeShapeDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        morphedShape = false;

        yield break;
    }

}
