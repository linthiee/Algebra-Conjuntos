using UnityEngine;

public class Rectangle
{
    public float x;
    public float y;
    public float z;
    public float width;
    public float height;
    public float depth;

    public bool Contains(GameObject gameObject, int dimensions)
    {
        float halfWidth = width * 0.5f;
        float halfHeight = height * 0.5f;
        float halfDepth = depth * 0.5f;

        Vector3 pos = gameObject.transform.position;

        bool insideXY = (pos.x >= x - halfWidth &&
                         pos.x <= x + halfWidth &&
                         pos.y >= y - halfHeight &&
                         pos.y <= y + halfHeight);

        if (dimensions == 2)
            return insideXY;

        bool insideZ = (pos.z >= z - halfDepth &&
                        pos.z <= z + halfDepth);

        return insideXY && insideZ;
    }
}
