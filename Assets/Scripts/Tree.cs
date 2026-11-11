using System;
using UnityEngine;

public class Tree  
{
    private int _dimensions;

    public Rectangle Boundary { get; }

    public Tree(int dimensions, Rectangle boundary)
    {
        this._dimensions = dimensions;
        this.Boundary = boundary;
    }
}