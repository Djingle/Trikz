using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothMovement
{

    public static float SmoothSin(float x)
    {
        return .5f * Mathf.Sin(x - Mathf.PI / 2f) + .5f;
    }

    public static Matrix4x4 MatrixLerp(Matrix4x4 from, Matrix4x4 to, float time)
    {
        Matrix4x4 ret = new Matrix4x4();
        for (int i = 0; i < 16; i++)
            ret[i] = Mathf.Lerp(from[i], to[i], time);
        return ret;
    }
}
