using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Hacky utilties class... :+
public class Gradient2RampTexture : MonoBehaviour
{
    public Gradient gradient;
    public Texture2D ramp_texture;

    public string texture_path;
    public Vector2Int resolution;

    // Start is called before the first frame update
    void Start()
    {
        gradient = GetComponent<Gradient>();

    }

    public void SaveGradientImage()
    {
        
        Color[] colors = new Color[resolution.x];

        ramp_texture = new Texture2D(resolution.x, resolution.y, TextureFormat.RGBA32, false);

        for(int i = 0; i < resolution.x; i++)
        {
            colors[i] = gradient.Evaluate(i / (float)resolution.x);
        }

        ramp_texture.SetPixels(colors);

        WriteImage(ramp_texture);
    }
    public void SaveRandomImage()
    {

        Color[] colors = new Color[resolution.x * resolution.y];

        ramp_texture = new Texture2D(resolution.x, resolution.y, TextureFormat.RGBA32, false);

        for (int i = 0; i < resolution.x * resolution.y; i++)
        {
            colors[i] = new Color(Random.value, Random.value, Random.value, Random.value);
        }

        ramp_texture.SetPixels(colors);

        WriteImage(ramp_texture);
    }

    private void WriteImage(Texture2D image)
    {
        byte[] bytesstrip = image.EncodeToPNG();
        System.IO.File.WriteAllBytes(texture_path, bytesstrip);
    }
}
