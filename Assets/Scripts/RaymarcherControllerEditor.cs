using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(RaymarcherController))]
public class RaymarcherControllerEditor : Editor
{

    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();

        RaymarcherController script = (RaymarcherController)target;

        if (GUILayout.Button("Change Shape"))
        {
            script.ChangeShape(script.test_target_shape);
        }

    }



}
#endif