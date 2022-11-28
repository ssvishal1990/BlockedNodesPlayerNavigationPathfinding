using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class stores the position of an grid object
/// </summary>
public class GridPosition 
{
    public int x;
    public int z;


    public int fcost = 0;
    public int gcost = 0;
    public int hcost = 0;

    public GridPosition(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public void calculateFcost()
    {
        fcost = gcost + hcost;
    }

    public override bool Equals(object obj)
    {
        return obj is GridPosition position &&
               x == position.x &&
               z == position.z;
    }

    public static GridPosition operator +(GridPosition a, GridPosition b)
    {
        return new GridPosition(a.x + b.x, a.z + b.z);
    }

    public static GridPosition operator +(GridPosition a, Vector2Int b)
    {
        return new GridPosition(a.x + b.x, a.z + b.y);
    }


    public override int GetHashCode()
    {
        return HashCode.Combine(x, z);
    }

    public override string ToString()
    {
        return $"x : { x }, y : { z } ";
    }
}
