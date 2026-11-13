using UnityEngine;

public class Cube
{
    public GameObject _cube;

    private Vector3 _extents;
    private float _broadRadius;
    public bool isColliding;
    public bool isCandidate;

    public Cube(GameObject cube)
    {
        _cube = cube;
    }

    public void CalculateRadius()
    {
        _extents = _cube.transform.localScale * 0.5f;
        _broadRadius = _extents.magnitude * 2.0f;
    }

    public bool CheckCollisionRadius(Cube objectB)
    {
        Vector3 posA = _cube.transform.position;
        Vector3 posB = objectB._cube.transform.position;

        float deltaX = posA.x - posB.x;
        float deltaY = posA.y - posB.y;
        float deltaZ = posA.z - posB.z;

        float sqrDistance = (deltaX * deltaX) + (deltaY * deltaY) + (deltaZ * deltaZ);
        float radiusSum = _broadRadius + objectB._broadRadius;

        return sqrDistance <= (radiusSum * radiusSum);
    }
    
    public bool CheckCollisionAABB(Cube objectB, int dimensions)
    {
        Vector3 posA = _cube.transform.position;
        Vector3 scaleA = _cube.transform.localScale;
        
        float halfWidthA = scaleA.x * 0.5f;
        float halfHeightA = scaleA.y * 0.5f;
        float halfDepthA = scaleA.z * 0.5f;

        Vector3 posB = objectB._cube.transform.position;
        Vector3 scaleB = objectB._cube.transform.localScale;
        
        float halfWidthB = scaleB.x * 0.5f;
        float halfHeightB = scaleB.y * 0.5f;
        float halfDepthB = scaleB.z * 0.5f;

        bool insideXY = (posA.x + halfWidthA >= posB.x - halfWidthB &&
                         posA.x - halfWidthA <= posB.x + halfWidthB &&
                         posA.y + halfHeightA >= posB.y - halfHeightB &&
                         posA.y - halfHeightA <= posB.y + halfHeightB);

        if (dimensions == 2)
            return insideXY;

        bool insideZ = (posA.z + halfDepthA >= posB.z - halfDepthB &&
                        posA.z - halfDepthA <= posB.z + halfDepthB);

        return insideXY && insideZ;
    }
    
    public void DrawRadius()
    {
        if (isColliding)
        {
            Gizmos.color = Color.red;
        }
        else if (isCandidate)
        {
            Gizmos.color = Color.yellow; 
        }
        else
        {
            Gizmos.color = Color.green; 
        }

        Vector3 center = _cube.transform.position;
        Vector3 vertexOffset = _extents * 2.0f;
        Vector3 targetPoint = center + vertexOffset;

        Gizmos.DrawLine(center, targetPoint);
        Gizmos.DrawWireSphere(center, _broadRadius);

        if (isCandidate || isColliding)
        {
            Gizmos.DrawWireCube(_cube.transform.position, _cube.transform.localScale);
        }
    }
}