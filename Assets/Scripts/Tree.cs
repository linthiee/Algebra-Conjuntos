using System;
using System.Collections.Generic;
using UnityEngine;

public class Tree
{
    private readonly int _dimensions;
    private readonly int _capacity;
    private readonly List<GameObject> _objects = new List<GameObject>();
    public Rectangle Boundary { get; }

    private int _childSize;
    private Tree[] _children;

    private bool _isDivided;

    private static readonly Vector3[] ChildDirections = new Vector3[]
    {
        new Vector3(-1.0f, 1.0f, -1.0f), //arriba izquierda (atras) / octree y quadtree
        new Vector3(1.0f, 1.0f, -1.0f), //arriba derecha (atras)
        new Vector3(-1.0f, -1.0f, -1.0f), //abajo izquierda (atras)
        new Vector3(1.0f, -1.0f, -1.0f), //abajo derecha (atras)

        new Vector3(-1.0f, 1.0f, 1.0f), //arriba izquierda (adelante) / octree
        new Vector3(1.0f, 1.0f, 1.0f), //arriba derecha (adelante)
        new Vector3(-1.0f, -1.0f, 1.0f), //abajo izquierda (adelante)
        new Vector3(1.0f, -1.0f, 1.0f) //abajo derecha (adelante)
    };

    public Tree(int dimensions, Rectangle boundary, int capacity)
    {
        this._dimensions = dimensions;
        this.Boundary = boundary;
        this._capacity = capacity;
        _isDivided = false;
    }

    public void Insert(GameObject gameObject)
    {
        Debug.Log("Entrando");

        if (!Boundary.Contains(gameObject))
        {
            Debug.Log("Esta fuera de mis limites");

            return;
        }
        
        if (_objects.Count < _capacity)
        {
            Debug.Log("Agregando");

            _objects.Add(gameObject);
        }
        else
        {
            Debug.Log("Estoy lleno, se lo doy a mis hijos");

            if (!_isDivided)
            {
                Debug.Log("Dividiendome");
                Subdivide();
            }

            for (int i = 0; i < _childSize; i++)
                _children[i].Insert(gameObject);
        }
    }

    private void Subdivide()
    {
        _isDivided = true;

        _childSize = _dimensions == 2 ? 4 : 8;
        _children = new Tree[_childSize];

        float childWidth = Boundary.width * 0.5f;
        float childHeight = Boundary.height * 0.5f;

        float offsetX = childWidth * 0.5f;
        float offsetY = childHeight * 0.5f;

        for (int i = 0; i < _childSize; i++)
        {
            Rectangle childBoundary = new Rectangle();

            childBoundary.x = Boundary.x + (offsetX * ChildDirections[i].x);
            childBoundary.y = Boundary.y + (offsetY * ChildDirections[i].y);
            childBoundary.width = childWidth;
            childBoundary.height = childHeight;

            _children[i] = new Tree(_dimensions, childBoundary, _capacity);
        }
    }
    
    public void DrawGizmos()
    {
        Gizmos.color = Color.green;

        Vector3 center = new Vector3(Boundary.x, Boundary.y, 0.0f);
        Vector3 size = new Vector3(Boundary.width, Boundary.height, 0.0f);

        Gizmos.DrawWireCube(center, size);

        if (_children != null)
        {
            for (int i = 0; i < _children.Length; i++)
            {
                if (_children[i] != null)
                {
                    _children[i].DrawGizmos();
                }
            }
        }
    }
}