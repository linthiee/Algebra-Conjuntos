using UnityEngine;

public class Tester : MonoBehaviour
{
    private Tree _quadTree;
    private Rectangle _boundary;

    public void Start()
    {
       _boundary.x = 200.0f;
       _boundary.y = 200.0f;
       _boundary.width = 200.0f;
       _boundary.height = 200.0f;
        
       _quadTree = new Tree(2, _boundary);
    }

    public void Update()
    {
    }
    
    public void OnDrawGizmos()
    {
        if (_quadTree == null)
            return;

        Gizmos.color = Color.green;
        
        Vector3 center = new Vector3(_quadTree.Boundary.x, _quadTree.Boundary.y, 0.0f);
        Vector3 size = new Vector3(_quadTree.Boundary.width, _quadTree.Boundary.height, 0.0f);
        
        Gizmos.DrawWireCube(center, size);
    }
}
