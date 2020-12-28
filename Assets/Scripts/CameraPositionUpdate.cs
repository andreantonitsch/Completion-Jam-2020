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
    void Update()
    {
        Shader.SetGlobalVector("_CameraPosition", transform.position);
        //Change to quaternion math
        Shader.SetGlobalVector("_CameraOrientation", transform.rotation.eulerAngles);
        Shader.SetGlobalVector("_CameraTarget", Target.position);


    }
}
