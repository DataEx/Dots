using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBox : MonoBehaviour {

    [SerializeField]
    Coordinate coordinate;
    public Coordinate Coordinate
    {
        get
        {
            return coordinate;
        }
        set
        {
            coordinate = value;
            Globals.Grid.UpdateGrid(this, coordinate);
        }
    }

    private Dot gridDot;
    public Dot GridDot
    {
        get
        {
            return gridDot;
        }
    }

    // Whether is currently falling
    public bool isUpdatingCoordinate = false;



    public void Initialize(Coordinate c, Vector3 spawnLocation)
    {
        coordinate = c;
        this.transform.position = spawnLocation;
        this.name = string.Format("Gridbox: ({0}, {1})", c.X, c.Y);
    }

	// Use this for initialization
	void Start () {
        CreateDot();
    }

    public void CreateDot()
    {
        gridDot = Instantiate(Globals.DotPrefab);
        gridDot.transform.parent = this.transform;
        gridDot.transform.localPosition = new Vector3(0, 0, Globals.GridBoxZOffset);
        gridDot.Color = Globals.GetRandomColor();
        gridDot.GridBox = this;
    }

    // Used when starting location is above the grid
    public void FallDownFromAbove()
    {
        isUpdatingCoordinate = true;
        Vector3 newLocation = GridController.GetSpawnLocation(coordinate);
        StartCoroutine(FallDownCoroutine(newLocation));
    }

    // Used when starting location is already in the grid
    public void FallDown(int numberOfSpacesToFall)
    {
        isUpdatingCoordinate = true;
        Coordinate = new Coordinate(coordinate.X, coordinate.Y + numberOfSpacesToFall);
        Vector3 newLocation = GridController.GetSpawnLocation(coordinate);
        StartCoroutine(FallDownCoroutine(newLocation));
    }


    // "Animation" for falling dots
    IEnumerator FallDownCoroutine(Vector3 destination)
    {
        yield return null;

        Vector3 currentLocation = this.transform.position;
        while (currentLocation.y > destination.y)
        {
            currentLocation.y -= Time.deltaTime * Globals.FallingSpeed;
            this.transform.position = currentLocation;
            yield return null;
        }
        this.transform.position = destination;
        isUpdatingCoordinate = false;
    }
}
