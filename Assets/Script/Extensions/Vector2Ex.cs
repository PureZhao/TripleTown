using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector2Ex
{
    public static bool IsPerpendicularTo(this Vector2 from, Vector2 to)
    {
        return Vector2.Dot(from, to) < 0.00001f;
    }

    public static bool IsParallelWith(this Vector2 from, Vector2 to)
    {
        Vector2 minus = from.normalized - to.normalized;
        minus.x = Mathf.Abs(minus.x);
        minus.y = Mathf.Abs(minus.y);
        return minus.x <= 0.00001f && minus.y <= 0.00001f;
    }
}
