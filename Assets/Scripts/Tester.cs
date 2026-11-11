using UnityEngine;

public class Tester : MonoBehaviour
{
    private Tree _quadTree;
    private Rectangle _boundary;
    private int _capacity;
    public void Start()
    {
        _boundary = new Rectangle();
        
       _boundary.x = 200.0f;
       _boundary.y = 200.0f;
       _boundary.width = 200.0f;
       _boundary.height = 200.0f;

       _capacity = 4;
       
       _quadTree = new Tree(3, _boundary, _capacity);
       
        InsertItems();
       
    }

    public void Update()
    {
        if (_quadTree == null)
            return;
        
    }
    
    public void OnDrawGizmos()
    {
        if (_quadTree == null)
            return;

        _quadTree.DrawGizmos();
    }

    private void InsertItems()
    {
        float halfWidth = _quadTree.Boundary.width * 0.5f;
        float halfHeight = _quadTree.Boundary.height * 0.5f;

        float minX = _quadTree.Boundary.x - halfWidth;
        float maxX = _quadTree.Boundary.x + halfWidth;

        float minY = _quadTree.Boundary.y - halfHeight;
        float maxY = _quadTree.Boundary.y + halfHeight;

        for (int i = 0; i < 50; i++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);

            cube.transform.position = new Vector3(randomX, randomY, 0.0f);
            cube.transform.localScale = Vector3.one * Random.Range(3.0f, 5.0f);
            
            _quadTree.Insert(cube);
        }
    }
}

