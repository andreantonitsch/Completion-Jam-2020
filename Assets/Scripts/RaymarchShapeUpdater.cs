using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RaymarchShapeUpdater : MonoBehaviour
{
    public Transform t;
    public Material raymarch_material;

    
    // Update is called once per frame
    void FixedUpdate()
    {
        raymarch_material.SetVector("_JelloPosition", t.position);
        raymarch_material.SetVector("_JelloRotation", t.rotation.eulerAngles * Mathf.Deg2Rad);
        raymarch_material.SetVector("_JelloScale", t.localScale);

        Quaternion rotQ = t.localRotation;
        Vector3 sclV = t.localScale;

        Matrix4x4 mat = Matrix4x4.TRS(t.position, rotQ, sclV);
        //GetComponent<Renderer>().sharedMaterial.SetMatrix("_UVWTransform", mat);

        raymarch_material.SetMatrix("_TRS", mat);

    }
}
