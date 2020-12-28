using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public enum SHAPE
    {
        SPHERE, CUBE
    }

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

        Player = shapeList[currentShape];
        PlayerRig = Player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        if (InputManager.instance.pressE && !morphedShape)
        {
            Vector3 lastVel = Player.transform.TransformDirection(PlayerRig.velocity);
            Vector3 lastAngVel = Player.transform.TransformDirection(PlayerRig.angularVelocity);

            Quaternion lastRot = Player.transform.rotation;

            if (currentShape < shapeList.Count - 1)
            {
                shapeList[currentShape].SetActive(false);
               
                currentShape++;
                shapeList[currentShape].SetActive(true);
                shapeList[currentShape].transform.position = shapeList[currentShape-1].transform.transform.position;

            }
            else
            {
                shapeList[currentShape].SetActive(false);
                currentShape = 0;
                shapeList[currentShape].SetActive(true);
                shapeList[currentShape].transform.position = shapeList[shapeList.Count - 1].transform.transform.position;
            }


            if(currentShape == (int)SHAPE.SPHERE)
            {
                MovementController.instance.forceMult = 50;   
            }

            if (currentShape == (int)SHAPE.CUBE)
            {
                MovementController.instance.forceMult = 35;
            }

            Player = shapeList[currentShape];
            Player.transform.rotation = lastRot;
            PlayerRig = Player.GetComponent<Rigidbody>();
            PlayerRig.velocity = Player.transform.InverseTransformDirection(lastVel);
            PlayerRig.angularVelocity = Player.transform.InverseTransformDirection(lastAngVel);
            virtCamera.Follow = Player.transform;

            morphedShape = true;

            StartCoroutine(ChangeShapeDelay(2f));

        }

    }


    IEnumerator ChangeShapeDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        morphedShape = false;

        yield break;
    }

}
