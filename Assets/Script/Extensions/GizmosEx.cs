using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class GizmosEx
{
    /// <summary>
    /// 在一个地方画数字, 默认每个数字之间间隔0.05
    /// </summary>
    /// <param name="number">数字</param>
    /// <param name="upLeft">数字的左上角点</param>
    /// <param name="sizeOfPerDigit">数字的大小，x为height，y为width</param>
    /// <param name="color">数字颜色</param>
    public static void DrawNumber<T>(T number, Vector3 upLeft, Vector2? sizeOfPerDigit = null)
    {
        if(sizeOfPerDigit == null)
        {
            sizeOfPerDigit = new Vector2(0.2f, 0.1f);
        }
        Vector2 size = (Vector2)sizeOfPerDigit;
        string digits = number.ToString();
        Vector3 nextUpLeft = Vector3.zero;
        for (int i = 0; i < digits.Length; i++)
        {
            if (i == 0)
            {
                nextUpLeft = upLeft;
            }
            else
            {
                nextUpLeft = nextUpLeft + Vector3.right * (size.y + 0.05f);
            }

            char c = digits[i];
            switch (c)
            {
                case '0': DrawNumber0(nextUpLeft, size); break;
                case '1': DrawNumber1(nextUpLeft, size); break;
                case '2': DrawNumber2(nextUpLeft, size); break;
                case '3': DrawNumber3(nextUpLeft, size); break;
                case '4': DrawNumber4(nextUpLeft, size); break;
                case '5': DrawNumber5(nextUpLeft, size); break;
                case '6': DrawNumber6(nextUpLeft, size); break;
                case '7': DrawNumber7(nextUpLeft, size); break;
                case '8': DrawNumber8(nextUpLeft, size); break;
                case '9': DrawNumber9(nextUpLeft, size); break;
                case '.': DrawDot(nextUpLeft, size); break;
            }
        }
    }
    static void DrawNumber0(Vector3 upLeft, Vector2 size)
    {
        Vector3[] vertices = new Vector3[]
        {
            upLeft,
            upLeft + Vector3.down * size.x,
            upLeft + Vector3.down * size.x + Vector3.right * size.y,
            upLeft + Vector3.right * size.y,
            upLeft,
        };
        for(int i = 1; i < vertices.Length; i++)
        {
            Gizmos.DrawLine(vertices[i - 1], vertices[i]);
        }
    }
    static void DrawNumber1(Vector3 upLeft, Vector2 size)
    {
        Vector3[] vertices = new Vector3[]
        {
            upLeft + Vector3.right * size.y * 0.25f + Vector3.down * size.x * 0.25f,
            upLeft + Vector3.right * size.y * 0.5f,
            upLeft + Vector3.right * size.y * 0.5f + Vector3.down * size.x,
            upLeft + Vector3.right * size.y * 0.25f + Vector3.down * size.x,
            upLeft + Vector3.right * size.y * 0.75f + Vector3.down * size.x,
        };
        for (int i = 1; i < vertices.Length; i++)
        {
            Gizmos.DrawLine(vertices[i - 1], vertices[i]);
        }
    }
    static void DrawNumber2(Vector3 upLeft, Vector2 size)
    {
        Vector3[] vertices = new Vector3[]
        {
            upLeft,
            upLeft + Vector3.right * size.y,
            upLeft + Vector3.right * size.y + Vector3.down * size.x * 0.5f,
            upLeft + Vector3.down * size.x * 0.5f,
            upLeft + Vector3.down * size.x,
            upLeft + Vector3.down * size.x + Vector3.right * size.y,
        };
        for (int i = 1; i < vertices.Length; i++)
        {
            Gizmos.DrawLine(vertices[i - 1], vertices[i]);
        }
    }
    static void DrawNumber3(Vector3 upLeft, Vector2 size)
    {
        Vector3[] vertices = new Vector3[]
        {
            upLeft,
            upLeft + Vector3.right * size.y,
            upLeft + Vector3.right * size.y + Vector3.down * size.x * 0.5f,
            upLeft + Vector3.down * size.x * 0.5f,
            upLeft + Vector3.right * size.y + Vector3.down * size.x * 0.5f,
            upLeft + Vector3.down * size.x + Vector3.right * size.y,
            upLeft + Vector3.down * size.x,
        };
        for (int i = 1; i < vertices.Length; i++)
        {
            Gizmos.DrawLine(vertices[i - 1], vertices[i]);
        }
    }
    static void DrawNumber4(Vector3 upLeft, Vector2 size)
    {
        Vector3[] vertices = new Vector3[]
        {
            upLeft,
            upLeft + Vector3.down * size.x * 0.5f,
            upLeft + Vector3.down * size.x * 0.5f + Vector3.right * size.y,
            upLeft + Vector3.right * size.y,
            upLeft + Vector3.down * size.x + Vector3.right * size.y,
        };
        for (int i = 1; i < vertices.Length; i++)
        {
            Gizmos.DrawLine(vertices[i - 1], vertices[i]);
        }
    }
    static void DrawNumber5(Vector3 upLeft, Vector2 size)
    {
        Vector3[] vertices = new Vector3[]
        {
            upLeft + Vector3.right * size.y,
            upLeft,
            upLeft + Vector3.down * size.x * 0.5f,
            upLeft + Vector3.down * size.x * 0.5f + Vector3.right * size.y,
            upLeft + Vector3.down * size.x + Vector3.right * size.y,
            upLeft + Vector3.down * size.x,
        };
        for (int i = 1; i < vertices.Length; i++)
        {
            Gizmos.DrawLine(vertices[i - 1], vertices[i]);
        }
    }
    static void DrawNumber6(Vector3 upLeft, Vector2 size)
    {
        Vector3[] vertices = new Vector3[]
        {
            upLeft + Vector3.right * size.y,
            upLeft,
            upLeft + Vector3.down * size.x,
            upLeft + Vector3.down * size.x + Vector3.right * size.y,
            upLeft + Vector3.down * size.x * 0.5f + Vector3.right * size.y,
            upLeft + Vector3.down * size.x * 0.5f,
        };
        for (int i = 1; i < vertices.Length; i++)
        {
            Gizmos.DrawLine(vertices[i - 1], vertices[i]);
        }
    }
    static void DrawNumber7(Vector3 upLeft, Vector2 size)
    {
        Vector3[] vertices = new Vector3[]
        {
            upLeft,
            upLeft + Vector3.right * size.y,
            upLeft + Vector3.down * size.x + Vector3.right * size.y,
        };
        for (int i = 1; i < vertices.Length; i++)
        {
            Gizmos.DrawLine(vertices[i - 1], vertices[i]);
        }
    }
    static void DrawNumber8(Vector3 upLeft, Vector2 size)
    {
        Vector3[] vertices = new Vector3[]
        {
            upLeft,
            upLeft + Vector3.down * size.x,
            upLeft + Vector3.down * size.x + Vector3.right * size.y,
            upLeft + Vector3.right * size.y,
            upLeft,
            upLeft + Vector3.down * size.x * 0.5f,
            upLeft + Vector3.down * size.x * 0.5f + Vector3.right * size.y,
        };
        for (int i = 1; i < vertices.Length; i++)
        {
            Gizmos.DrawLine(vertices[i - 1], vertices[i]);
        }
    }
    static void DrawNumber9(Vector3 upLeft, Vector2 size)
    {
        Vector3[] vertices = new Vector3[]
        {
            upLeft + Vector3.down * size.x,
            upLeft + Vector3.down * size.x + Vector3.right * size.y,
            upLeft + Vector3.right * size.y,
            upLeft,
            upLeft + Vector3.down * size.x * 0.5f,
            upLeft + Vector3.down * size.x * 0.5f + Vector3.right * size.y,
        };
        for (int i = 1; i < vertices.Length; i++)
        {
            Gizmos.DrawLine(vertices[i - 1], vertices[i]);
        }
    }
    static void DrawDot(Vector3 upLeft, Vector2 size)
    {
        float minSide = Mathf.Min(size.y, size.x);
        float side = minSide * 0.25f;
        float thickness = 0.001f;
        Vector3 center = upLeft + Vector3.right * size.y * 0.5f;
        center += Vector3.down * (size.x - side / 2f);
        Gizmos.DrawCube(center, new Vector3(side, side, thickness));
    }
    /// <summary>
    /// 画一个胶囊体框架
    /// </summary>
    /// <param name="center">胶囊体中心</param>
    /// <param name="forward">胶囊体前方向，此处为radius的方向</param>
    /// <param name="up">胶囊体上方，此处为height的方向</param>
    /// <param name="radius">胶囊体半径</param>
    /// <param name="height">胶囊体高度</param>
    /// <param name="stepAngle">胶囊体中圆的平滑度</param>
    public static void DrawWireCapsule(Vector3 center, Vector3 forward, Vector3 up, float radius, float height, float stepAngle = 15f)
    {
        if (Vector3.Dot(forward, up) > 0.00001f || forward == up) return;
        Vector3 right = Vector3.Cross(forward, up);
        radius = Mathf.Abs(radius);
        height = Mathf.Abs(height);
        stepAngle = stepAngle <= 5f ? 5 : stepAngle;
        stepAngle = stepAngle >= 45 ? 45 : stepAngle;
        if (radius == 0 || height == 0) return;
        if(radius * 2 >= height)
        {
            DrawWireCircle(center, forward, right, radius, stepAngle);
            DrawWireCircle(center, up, forward, radius, stepAngle);
            DrawWireCircle(center, right, up, radius, stepAngle);
        }
        else
        {
            float heightOfCylinder = height - 2 * radius;
            Vector3 upCircleCenter = center + up * heightOfCylinder * 0.5f;
            Vector3 downCircleCenter = center -up * heightOfCylinder * 0.5f;
            DrawWirePlaneCapsule(center, forward, up, radius, height, stepAngle);
            DrawWirePlaneCapsule(center, right, up, radius, height, stepAngle);
            DrawWireCircle(upCircleCenter, forward, right, radius, stepAngle);
            DrawWireCircle(downCircleCenter, forward, right, radius, stepAngle);
        }

    }
    // forward right 是以圆心为同一起点垂直的两个向量
    public static void DrawWireCircle(Vector3 center, Vector3 forward, Vector3 right, float radius, float stepAngle = 15f)
    {
        if (Vector3.Dot(forward, right) > 0.00001f || forward == right) return;
        radius = Mathf.Abs(radius);
        if (radius == 0) return;
        stepAngle = stepAngle <= 5 ? 5 : stepAngle;
        stepAngle = stepAngle >= 45 ? 45 : stepAngle;
        List<Vector3> vertices = new List<Vector3>();
        Vector3 startPoint = center + forward * radius;
        vertices.Add(startPoint);
        float angle = 0;
        while(angle < 360)
        {
            angle += stepAngle;

            if (angle <= 360)
            {
                float x = radius * Mathf.Cos(Mathf.Deg2Rad * angle);
                float y = radius * Mathf.Sin(Mathf.Deg2Rad * angle);
                Vector3 vertex = center + forward * x + right * y;
                vertices.Add(vertex);
            }
            else
            {
                vertices.Add(startPoint);
            }
        }
        for (int i = 1; i < vertices.Count; i++)
        {
            Gizmos.DrawLine(vertices[i - 1], vertices[i]);
        }
    }
    // forward right 是以圆心为同一起点垂直的两个向量
    public static void DrawWireHalfCircle(Vector3 center, Vector3 forward, Vector3 right, float radius, float stepAngle = 15f)
    {
        if (Vector3.Dot(forward, right) > 0.00001f || forward == right) return;
        radius = Mathf.Abs(radius);
        if (radius == 0) return;
        stepAngle = stepAngle <= 5 ? 5 : stepAngle;
        stepAngle = stepAngle >= 45 ? 45 : stepAngle;
        List<Vector3> vertices = new List<Vector3>();
        Vector3 startPoint = center + forward * radius;
        Vector3 endPoint = center - forward * radius;
        vertices.Add(startPoint);
        float angle = 0;
        while (angle < 180)
        {
            angle += stepAngle;
            if (angle <= 180)
            {
                float x = radius * Mathf.Cos(Mathf.Deg2Rad * angle);
                float y = radius * Mathf.Sin(Mathf.Deg2Rad * angle);
                Vector3 vertex = center + forward * x + right * y;
                vertices.Add(vertex);
            }
            else
            {
                vertices.Add(endPoint);
            }
        }
        for (int i = 1; i < vertices.Count; i++)
        {
            Gizmos.DrawLine(vertices[i - 1], vertices[i]);
        }
    }
    /// <param name="forward">胶囊体前方向，此处为radius的方向</param>
    /// <param name="up">胶囊体上方，此处为height的方向</param>
    public static void DrawWirePlaneCapsule(Vector3 center, Vector3 forward, Vector3 up, float radius, float height, float stepAngle = 15f)
    {
        if (Vector3.Dot(forward, up) > 0.00001f || forward == up) return;
        radius = Mathf.Abs(radius);
        height = Mathf.Abs(height);
        if (radius == 0 || height == 0) return;
        stepAngle = stepAngle <= 5 ? 5 : stepAngle;
        stepAngle = stepAngle >= 45 ? 45 : stepAngle;
        Vector3 right = Vector3.Cross(forward, up);
        if (radius * 2 >= height)
        {
            DrawWireCircle(center, forward, right, radius, stepAngle);
        }
        else
        {
            float heightOfCylinder = height - 2 * radius;
            Vector3 upCircleCenter = center + up * heightOfCylinder * 0.5f;
            Vector3 downCircleCenter = center - up * heightOfCylinder * 0.5f;
            DrawWireHalfCircle(downCircleCenter, -forward, -up, radius, stepAngle);
            DrawWireHalfCircle(upCircleCenter, forward, up, radius, stepAngle);
            Vector3[] vertices = new Vector3[]
            {
            upCircleCenter + forward * radius,
            upCircleCenter + forward * radius - up * heightOfCylinder,

            upCircleCenter - forward * radius,
            upCircleCenter - forward * radius - up * heightOfCylinder,

            upCircleCenter + right * radius,
            upCircleCenter + right * radius - up * heightOfCylinder,

            upCircleCenter - right * radius,
            upCircleCenter - right * radius - up * heightOfCylinder,

            };
            for (int i = 1; i < vertices.Length; i += 2)
            {
                Gizmos.DrawLine(vertices[i - 1], vertices[i]);
            }
        }
    }
    public static void DrawWireCylinder(Vector3 center, Vector3 forward, Vector3 up, float radius, float height, int divide = 16)
    {
        divide = Mathf.Abs(divide);
        if (Vector3.Dot(forward, up) > 0.00001f || forward == up || divide < 4) return;
        if (divide % 2 == 1) divide += 1;
        if (divide > 36) divide = 36;
        Vector3 right = Vector3.Cross(forward, up);
        float stepAngle = 360 / divide;
        List<Vector3> vertices = new List<Vector3>();
        Vector3 upCenter = center + up * height * 0.5f;
        Vector3 downCenter = center - up * height * 0.5f;
        Vector3 startPoint = upCenter + forward * radius;
        vertices.Add(startPoint);
        float angle = 0;
        while (angle < 360)
        {
            angle += stepAngle;

            if (angle <= 360)
            {
                float x = radius * Mathf.Cos(Mathf.Deg2Rad * angle);
                float y = radius * Mathf.Sin(Mathf.Deg2Rad * angle);
                Vector3 vertex = upCenter + forward * x + right * y;
                vertices.Add(vertex);
            }
            else
            {
                vertices.Add(startPoint);
            }
        }
        for (int i = 1; i < vertices.Count; i++)
        {
            Vector3 downVertex1 = vertices[i - 1] - up * height;
            Vector3 downVertex2 = vertices[i] - up * height;
            Gizmos.DrawLine(vertices[i - 1], vertices[i]);
            Gizmos.DrawLine(vertices[i - 1], downVertex1);
            Gizmos.DrawLine(downVertex1, downVertex2);
        }
        for (int i = 0; i < vertices.Count; i++)
        {
            Vector3 downVertex = vertices[i] - up * height;
            Gizmos.DrawLine(upCenter, vertices[i]);
            Gizmos.DrawLine(downCenter, downVertex);
        }
    }

    public static void DrawWireSector(Vector3 center, Vector3 forward, Vector3 right, float radius, float angle, float stepAngle = 15f)
    {
        if (Vector3.Dot(forward, right) > 0.00001f || forward == right) return;
        radius = Mathf.Abs(radius);
        angle = Mathf.Abs(angle);
        if (radius == 0 || angle == 0) return;
        stepAngle = stepAngle <= 5 ? 5 : stepAngle;
        stepAngle = stepAngle >= 45 ? 45 : stepAngle;
        float halfAngle = angle / 2;
        float deltaAngle = 0f;
        List<Vector3> leftVertices = new List<Vector3>();
        List<Vector3> rightVertices = new List<Vector3>();
        Vector3 startPoint = center + forward * radius;
        float endX = radius * Mathf.Cos(Mathf.Deg2Rad * halfAngle);
        float endY = radius * Mathf.Sin(Mathf.Deg2Rad * halfAngle);
        Vector3 leftEndPoint = center + forward * endX + right * endY;
        Vector3 rightEndPoint = center + forward * endX - right * endY;
        leftVertices.Add(startPoint);
        rightVertices.Add(startPoint);

        while (deltaAngle < halfAngle)
        {
            deltaAngle += stepAngle;
            if (deltaAngle <= halfAngle)
            {
                float x = radius * Mathf.Cos(Mathf.Deg2Rad * deltaAngle);
                float y = radius * Mathf.Sin(Mathf.Deg2Rad * deltaAngle);
                Vector3 leftVertex = center + forward * x + right * y;
                Vector3 rightVertex = center + forward * x - right * y;
                leftVertices.Add(leftVertex);
                rightVertices.Add(rightVertex);
            }
            else
            {
                leftVertices.Add(leftEndPoint);
                rightVertices.Add(rightEndPoint);
            }
        }
        for (int i = 1; i < leftVertices.Count; i++)
        {
            Gizmos.DrawLine(leftVertices[i - 1], leftVertices[i]);
        }
        for (int i = 1; i < rightVertices.Count; i++)
        {
            Gizmos.DrawLine(rightVertices[i - 1], rightVertices[i]);
        }
        Gizmos.DrawLine(center, leftEndPoint);
        Gizmos.DrawLine(center, rightEndPoint);
    }
}
