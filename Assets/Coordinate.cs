using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Coordinate {

    [SerializeField]
    int x;
    public int X
    {
        get
        {
            return x;
        }
    }

    [SerializeField]
    int y;
    public int Y
    {
        get
        {
            return y;
        }
    }

    public Coordinate(int xCoord, int yCoord)
    {
        x = xCoord;
        y = yCoord;
    }

    public bool IsNeighbor(Coordinate c)
    {
        // Out of bounds
        if (c.X < 0 || c.Y < 0 || c.X >= Globals.GridWidth || c.Y >= Globals.GridHeight)
            return false;

        // Next to each other, on the same row
        if (Mathf.Abs(c.X - this.X) == 1 && c.Y == this.Y)
        {
            return true;
        }

        // Next to each other, on the same column
        if (Mathf.Abs(c.Y - this.Y) == 1 && c.X == this.X)
        {
            return true;
        }

        return false;
    }
}
