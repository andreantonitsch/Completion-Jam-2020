using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraPositionUpdate : MonoBehaviour
{
    Transform transform;
    public Transform Target;
    // Start is called before the first frame update
    void Start()
    {
        transform = gameObject.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(transform.position);
        Shader.SetGlobalVector("_CameraPosition",Camera.main.transform.position);
        //Change to quaternion math
        Shader.SetGlobalVector("_CameraOrientation", Camera.main.transform.rotation.eulerAngles);
        //Shader.SetGlobalVector("_CameraTarget", Target.position);

        var t = Camera.main.transform.forward;
        Shader.SetGlobalVector("_CameraTarget", t);



    }
}
