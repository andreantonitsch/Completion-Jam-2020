using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Gradient2RampTexture))]
public class Gradient2RampTextureEditor : Editor
{

    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();

        Gradient2RampTexture gradient_script = (Gradient2RampTexture)target;
        if (GUILayout.Button("Save Gradient Image"))
        {
            gradient_script.SaveGradientImage();
        }
        if (GUILayout.Button("Save Random Image"))
        {
            gradient_script.SaveRandomImage();
        }

    }



}
