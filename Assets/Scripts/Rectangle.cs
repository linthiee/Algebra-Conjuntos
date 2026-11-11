using UnityEngine;

public class Rectangle
{
    public float x;
    public float y;
    public float width;
    public float height;

    public bool Contains(GameObject gameObject)
    {
        float halfWidth = width * 0.5f;
        float halfHeight = height * 0.5f;

        Vector3 pos = gameObject.transform.position;
        Vector3 cubeHalfSize = gameObject.transform.localScale * 0.5f;

        return (pos.x + cubeHalfSize.x >= x - halfWidth &&
                pos.x - cubeHalfSize.x <= x + halfWidth &&
                pos.y + cubeHalfSize.y >= y - halfHeight &&
                pos.y - cubeHalfSize.y <= y + halfHeight);
    }
}
