using UnityEngine;
using UnityEngine.InputSystem;

public class Tester : MonoBehaviour
{
    [SerializeField] private int dimensions = 3;

    private Tree _quadTree;
    private Rectangle _boundary;
    private int _capacity;
    private Camera _camera;
    private int _cubeCount = 0;
    
    public void Start()
    {
        _boundary = new Rectangle();

        _boundary.x = 200.0f;
        _boundary.y = 200.0f;
        _boundary.z = 200.0f;

        _boundary.width = 200.0f;
        _boundary.height = 200.0f;
        _boundary.depth = 200.0f;

        _capacity = 4;

        _quadTree = new Tree(dimensions, _boundary, _capacity);

        _camera = Camera.main;
        
        if (dimensions == 3)
            SpawnCubes3D();
    }

    public void Update()
    {
        if (_quadTree == null)
            return;

        if (dimensions == 2 && Mouse.current.leftButton.wasPressedThisFrame)
            SpawnCubeAtMouse();
        
        _quadTree.ResetCollisions();
        _quadTree.CheckAABB();
    }

    public void OnDrawGizmos()
    {
        if (_quadTree == null)
            return;

        _quadTree.DrawGizmos();
    }

    private void SpawnCubeAtMouse()
    {
        float halfWidth = _quadTree.Boundary.width * 0.5f;
        float halfHeight = _quadTree.Boundary.height * 0.5f;

        float minX = _quadTree.Boundary.x - halfWidth;
        float maxX = _quadTree.Boundary.x + halfWidth;

        float minY = _quadTree.Boundary.y - halfHeight;
        float maxY = _quadTree.Boundary.y + halfHeight;
        
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Ray ray = _camera.ScreenPointToRay(mouseScreenPos);

        Vector3 treeCenter = new Vector3(_quadTree.Boundary.x, _quadTree.Boundary.y, 0.0f);

        Plane targetPlane = new Plane(-_camera.transform.forward, treeCenter);

        if (targetPlane.Raycast(ray, out float enterDistance))
        {
            Vector3 hitPoint = ray.GetPoint(enterDistance);

            float clampedX = Mathf.Clamp(hitPoint.x, minX, maxX);
            float clampedY = Mathf.Clamp(hitPoint.y, minY, maxY);

            Cube cube = new Cube(GameObject.CreatePrimitive(PrimitiveType.Cube));
            cube._cube.name = "Cube " + _cubeCount++;

            cube._cube.transform.position = new Vector3(clampedX, clampedY, 0.0f);
            cube._cube.transform.localScale = Vector3.one * Random.Range(3.0f, 5.0f);

            cube.CalculateRadius();
            _quadTree.Insert(cube);
        }
    }

    private void SpawnCubes3D()
    {
        float halfWidth = _quadTree.Boundary.width * 0.5f;
        float halfHeight = _quadTree.Boundary.height * 0.5f;
        float halfDepth = _quadTree.Boundary.depth * 0.5f;

        float minX = _quadTree.Boundary.x - halfWidth;
        float maxX = _quadTree.Boundary.x + halfWidth;

        float minY = _quadTree.Boundary.y - halfHeight;
        float maxY = _quadTree.Boundary.y + halfHeight;

        float minZ = _quadTree.Boundary.z - halfDepth;
        float maxZ = _quadTree.Boundary.z + halfDepth;

        for (int i = 0; i < 10; i++)
        {
            Cube cube = new Cube(GameObject.CreatePrimitive(PrimitiveType.Cube));

            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);
            float randomZ = Random.Range(minZ, maxZ);

            cube._cube.transform.position = new Vector3(randomX, randomY, randomZ);
            cube._cube.transform.transform.localScale = Vector3.one * Random.Range(3.0f, 5.0f);

            cube._cube.name = "Cube " + _cubeCount++;
            
            cube.CalculateRadius();
            _quadTree.Insert(cube);
        }
    }
}