using UnityEngine;

public class Cube
{
    public GameObject _cube;

    private Vector3 _extents;
    private float _broadRadius;

    public Cube(GameObject cube)
    {
        _cube = cube;
    }

    public void CalculateRadius()
    {
        _extents = _cube.transform.localScale * 0.5f;
        _broadRadius = _extents.magnitude * 2.0f;
    }

    public void DrawRadius()
    {
        Gizmos.color = Color.yellow;

        Vector3 center = _cube.transform.position;
        Vector3 vertexOffset = _extents * 2.0f;
        Vector3 targetPoint = center + vertexOffset;

        Gizmos.DrawLine(center, targetPoint);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center, _broadRadius);
    }
}