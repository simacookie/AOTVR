using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MathMap
{
    public static float Map(float value, float in_min, float in_max, float out_min, float out_max, bool clamp = false)
    {
        if (clamp) value = Mathf.Max(in_min, Mathf.Min(value, in_max));
        return (value - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }

}
