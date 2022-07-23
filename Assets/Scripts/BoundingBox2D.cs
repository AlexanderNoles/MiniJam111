using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoundingBox2D
{
    public Vector3 center;
    public Vector2 extents;
    public Color color;

    public void Draw()
    {
        Debug.DrawLine(center + V2toV3(extents), new Vector3(center.x - extents.x, center.y + extents.y), color);
        Debug.DrawLine(center + V2toV3(extents), new Vector3(center.x + extents.x, center.y - extents.y), color);
        Debug.DrawLine(new Vector3(center.x + extents.x, center.y - extents.y), center - V2toV3(extents), color);
        Debug.DrawLine(center - V2toV3(extents), new Vector3(center.x - extents.x, center.y + extents.y), color);
    }

    public Vector3 V2toV3(Vector2 _vector)
    {
        return new Vector3(_vector.x, _vector.y);
    }
}
