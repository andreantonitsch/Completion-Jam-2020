using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeMorpher : MonoBehaviour
{
    public SHAPE shapeType;
    public GameObject shape;
    public GameObject twos;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        PlayerController.instance.shapeList[(int)shapeType] = shape;
        PlayerController.instance.ChangeShape((int)shapeType);

        //twos.SetActive(true);
    }

}
