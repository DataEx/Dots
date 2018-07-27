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

    public bool isUpdatingCoordinate = false;


    // TODO: shouldn't be public
    public Dot gridDot;

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
        gridDot = Instantiate(Globals.DotPrefab).GetComponent<Dot>();
        gridDot.transform.parent = this.transform;
        gridDot.transform.localPosition = new Vector3(0, 0, Globals.GridBoxZOffset);
        gridDot.Color = Globals.GetRandomColor();
        gridDot.GridBox = this;
    }

    private void OnDestroy()
    {
        print("Dest: " + coordinate.X + ", " + coordinate.Y);


    }

    public void FallDownFromAbove()
    {
        isUpdatingCoordinate = true;
        Vector3 newLocation = Grid.GetSpawnLocation(coordinate);
        StartCoroutine(FallDownCoroutine(newLocation));
    }

    public void FallDown(int numberOfSpacesToFall)
    {
        isUpdatingCoordinate = true;
        Coordinate = new Coordinate(coordinate.X, coordinate.Y + numberOfSpacesToFall);
        print(string.Format("new Coord {0},{1}", Coordinate.X, Coordinate.Y));

        Vector3 newLocation = Grid.GetSpawnLocation(coordinate);
        StartCoroutine(FallDownCoroutine(newLocation));
    }

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
