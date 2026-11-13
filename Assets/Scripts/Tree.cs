using System;
using System.Collections.Generic;
using UnityEngine;

public class Tree
{
    private readonly int _dimensions;
    private readonly int _capacity;
    private readonly List<Cube> _objects = new List<Cube>();
    public Rectangle Boundary { get; }
    public IReadOnlyList<Cube> Cubes => _objects;

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


    public void Insert(Cube cube)
    {
        Debug.Log("Entrando");

        if (!Boundary.Contains(cube._cube, _dimensions))
        {
            Debug.Log("Esta fuera de mis limites");

            return;
        }

        if (_objects.Count < _capacity && !_isDivided)
        {
            Debug.Log("Agregando");

            _objects.Add(cube);
            return;
        }

        Debug.Log("Estoy lleno, se lo doy a mis hijos");

        if (!_isDivided)
        {
            Debug.Log("Dividiendome");
            Subdivide();

            for (int i = 0; i < _objects.Count; i++)
              for (int j = 0; j < _childSize; j++)
                 _children[j].Insert(_objects[i]);
            
            _objects.Clear();
        }

        for (int i = 0; i < _childSize; i++)
            _children[i].Insert(cube);
    }

    private void Subdivide()
    {
        _isDivided = true;

        _childSize = _dimensions == 2 ? 4 : 8;
        _children = new Tree[_childSize];

        float childWidth = Boundary.width * 0.5f;
        float childHeight = Boundary.height * 0.5f;
        float childDepth = _dimensions == 3 ? Boundary.depth * 0.5f : 0.0f;

        float offsetX = childWidth * 0.5f;
        float offsetY = childHeight * 0.5f;
        float offsetZ = childDepth * 0.5f;

        for (int i = 0; i < _childSize; i++)
        {
            Rectangle childBoundary = new Rectangle();

            childBoundary.x = Boundary.x + (offsetX * ChildDirections[i].x);
            childBoundary.y = Boundary.y + (offsetY * ChildDirections[i].y);
            childBoundary.z = _dimensions == 3 ? Boundary.z + (offsetZ * ChildDirections[i].z) : 0.0f;

            childBoundary.width = childWidth;
            childBoundary.height = childHeight;
            childBoundary.depth = childDepth;

            _children[i] = new Tree(_dimensions, childBoundary, _capacity);
        }
    }

    public void DrawGizmos()
    {
        Gizmos.color = Color.green;

        float zCenter = _dimensions == 3 ? Boundary.z : 0.0f;
        float zSize = _dimensions == 3 ? Boundary.depth : 0.0f;

        Vector3 center = new Vector3(Boundary.x, Boundary.y, zCenter);
        Vector3 size = new Vector3(Boundary.width, Boundary.height, zSize);

        Gizmos.DrawWireCube(center, size);

        foreach (Cube cube in _objects)
        {
            if (cube != null)
            {
                cube.DrawRadius();
            }
        }

        if (_isDivided && _children != null)
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
    
    public void ResetCollisions()
    {
        for (int i = 0; i < _objects.Count; i++)
        {
            _objects[i].isCandidate = false;
            _objects[i].isColliding = false;
        }

        if (_isDivided && _children != null)
        {
            for (int i = 0; i < _children.Length; i++)
            {
                _children[i]?.ResetCollisions();
            }
        }
    }

    public void CheckAABB()
    {
        for (int i = 0; i < _objects.Count; i++)
        {
            for (int j = i + 1; j < _objects.Count; j++)
            {
                if (_objects[i].CheckCollisionRadius(_objects[j]))
                {
                    _objects[i].isCandidate = true;
                    _objects[j].isCandidate = true;
                    
                    if (_objects[i].CheckCollisionAABB(_objects[j], _dimensions))
                    {
                        _objects[i].isColliding = true;
                        _objects[j].isColliding = true;

                        Debug.Log($"Effectiver AABB collision between {_objects[i]._cube.name}  and {_objects[j]._cube.name}");
                    }
                }
            }
        }

        if (_isDivided && _children != null)
        {
            for (int i = 0; i < _children.Length; i++)
            {
                if (_children[i] != null)
                    _children[i].CheckAABB();
            }
        }
    }
}