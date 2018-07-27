﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    GridBox[,] grid;

    public static float GridBoxScale
    {
        get {
            return Globals.GridBoxPrefab.transform.localScale.z;
        }
    }


    private void Start()
    {
        grid = new GridBox[Globals.GridWidth, Globals.GridHeight];
        // TODO: Check if prefab is null before continuing
        for (int i = 0; i < Globals.GridWidth; i++)
        {
            for (int j = 0; j < Globals.GridHeight; j++)
            {
                GenerateGridBox(new Coordinate(i, j));
            }
        }
    }

    private GridBox GenerateGridBox(Coordinate c)
    {
        if (Globals.GridBoxPrefab == null)
        {
            print("No valid GridBox Prefab");
            return null;
        }
        GridBox gridBox = Instantiate(Globals.GridBoxPrefab).GetComponent<GridBox>();
        gridBox.Initialize(c, GetSpawnLocation(c));
        gridBox.transform.parent = this.transform;
        grid[c.X, c.Y] = gridBox;
        return gridBox;
    }

    public static Vector3 GetSpawnLocation(Coordinate c)
    {
        return new Vector3(c.X, -c.Y, 0) * (GridBoxScale * 10) + Globals.GridOrigin;
    }

    public void RepopulateGrid()
    {
        StartCoroutine(RepopulateGridCoroutine());
    }

    // Destroy old gridboxes, replace them, and handle falling animation
    IEnumerator RepopulateGridCoroutine()
    {
        // Need to wait a frame for the dots to be registered as destroyed
        yield return null;

        for (int i = Globals.GridWidth - 1; i >= 0; i--)
        {
            int dotsRemovedInColumn = 0;
            for (int j = Globals.GridHeight - 1; j >= 0; j--)
            {
                GridBox gridBox = grid[i, j];
                // Destroy old gridboxes
                if (gridBox != null && gridBox.gridDot == null)
                {
                    grid[i, j] = null;
                    Destroy(gridBox.gameObject);
                    dotsRemovedInColumn++;
                }
                else {
                    // Animate any gridboxes above the destroyed one falling down the screen
                    if (dotsRemovedInColumn > 0)
                        gridBox.FallDown(dotsRemovedInColumn);
                }
            }

            // Spawn {dotsRemovedInColumn} new dots in this column as replacements
            for (int j = dotsRemovedInColumn - 1; j >= 0; j--)
            {
                GridBox generatedGridBox = GenerateGridBox(new Coordinate(i, j));
                // Position new gridboxes above the screen
                generatedGridBox.transform.position = Grid.GetSpawnLocation(new Coordinate(i, j - dotsRemovedInColumn));
                generatedGridBox.FallDownFromAbove();
            }

        }
    }

    // Find next valid dot above the given location
    private GridBox GetNextActiveDotAbove(Coordinate coordinate)
    {
        for (int j = coordinate.Y - 1; j >= 0; j--)
        {
            GridBox aboveGridBox = grid[coordinate.X, j];
            if (aboveGridBox  != null && aboveGridBox.gridDot != null && !aboveGridBox.isUpdatingCoordinate)
            {
                return aboveGridBox;
            }
        }

        return null;
    }

    public void UpdateGrid(GridBox gridBox, Coordinate c)
    {
        try
        {
            grid[c.X, c.Y] = gridBox;
        }
        catch (IndexOutOfRangeException ex)
        {
            print(string.Format("Cannot access grid coordinate ({0},{1})", c.X, c.Y));
        }
    }

    public List<Dot> GetAllDotsOfColor(DotColor color)
    {
        List<Dot> dotsOfColor = new List<Dot>();
        for (int i = 0; i < Globals.GridWidth; i++)
        {
            for (int j = 0; j < Globals.GridHeight; j++)
            {
                GridBox gridBox = grid[i, j];
                if (gridBox != null && gridBox.gridDot != null && gridBox.gridDot.Color == color)
                {
                    dotsOfColor.Add(grid[i, j].gridDot);
                }
            }
        }
        return dotsOfColor;
    }
}