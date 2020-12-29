using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForce : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayerController.instance.PlayerRig.AddForce(transform.up * 100f, ForceMode.Impulse);
        }
    }
}
