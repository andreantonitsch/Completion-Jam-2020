using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraDraw : MonoBehaviour
{
    public Material renderMaterial;

    private Camera camera;


    private void Start()
    {
        camera = Camera.main;
        camera.depthTextureMode = DepthTextureMode.Depth;
    }

    protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, renderMaterial);
    }
}

