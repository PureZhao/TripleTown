using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Ex
{

    public static bool IsPerpendicularTo(this Vector3 from, Vector3 to)
    {
        return Vector3.Dot(from, to) < 0.00001f;
    }

    public static bool IsParallelWith(this Vector3 from, Vector3 to)
    {
        Vector3 minus = from.normalized - to.normalized;
        minus.x = Mathf.Abs(minus.x);
        minus.y = Mathf.Abs(minus.y);
        minus.z = Mathf.Abs(minus.z);
        return minus.x <= 0.00001f && minus.y <= 0.00001f && minus.z <= 0.00001f;
    }
}
