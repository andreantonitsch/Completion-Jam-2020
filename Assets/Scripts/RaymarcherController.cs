using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sinc Curve based on Inigo Quillez
// https://www.iquilezles.org/www/articles/functions/functions.htm

public class RaymarcherController : MonoBehaviour
{
    public enum Shape
    {
        Sphere,
        Rectangle,
        Pyramid
    }

    public Color[] CoreColors;
    public Color[] ShellColors;
    
    public float ShapeChangeSteps = 0.5f;
    public int ShapeBounciness = 2;
    public Shape current_shape;
    public Shape old_shape;
    public bool dirty_shape;
    public Vector4 ShapeVector = new Vector4(1.0f, 0.0f, 0.0f, 0.0f);
    public Material raymarch_material;

    public Shape test_target_shape;

    public float SincCurve(float t, float k)
    {
        float a =Mathf.PI * k * k * t ;
        return Mathf.Sin(a) / a;
    }

    //Jank
    public void UpdateShape(float t, Shape target_shape, Shape previous_shape)
    {
        float p = SincCurve(t, ShapeBounciness) * 1.7f;
        Vector4 new_shape_v = Vector4.zero;

        switch (target_shape)
        {
            case Shape.Sphere:
                new_shape_v.x = 1-p;
                break;
            case Shape.Rectangle:
                new_shape_v.y = 1-p;
                break;
            case Shape.Pyramid:
                new_shape_v.z = 1-p;
                break;
        }
        switch (previous_shape)
        {
            case Shape.Sphere:
                new_shape_v.x = p;
                break;
            case Shape.Rectangle:
                new_shape_v.y = p;
                break;
            case Shape.Pyramid:
                new_shape_v.z = p;
                break;
        }

        ShapeVector = new_shape_v;
    }

    public  IEnumerator ShapeRoutine()
    {
        float timer = 0.0f;
        while(timer < ShapeChangeSteps)
        {
            timer += Time.deltaTime;
            float t = timer / ShapeChangeSteps;
            UpdateShape(t, current_shape, old_shape);
            dirty_shape = true;
            yield return null;
        }
        UpdateShape(1.0f, current_shape, old_shape);
    }

    public void ChangeShape(Shape s)
    {
        if (s == current_shape)
            return;

        old_shape = current_shape;
        current_shape = s;

        StartCoroutine(ShapeRoutine());
    }


    void Update()
    {
        if (dirty_shape)
        {
            raymarch_material.SetVector("_JelloShape", ShapeVector);
            Color core_col = Color.black;
            Color shell_col = Color.black;

            for (int i = 0; i < CoreColors.Length; i++)
            {
                core_col = core_col + CoreColors[i] * ShapeVector[i];
                shell_col = shell_col + ShellColors[i] * ShapeVector[i];
            }

            raymarch_material.SetVector("_SDF2Color", core_col);
            raymarch_material.SetVector("_SDFColor", shell_col);

            dirty_shape = false;

        }
    }


}
