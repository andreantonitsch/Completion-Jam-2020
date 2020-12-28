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
        raymarch_material.SetVector("_JelloRotation", t.rotation.eulerAngles);
        raymarch_material.SetVector("_JelloScale", t.localScale);

    }
}
