using System.Collections;
using UnityEngine;

public class GridBox : MonoBehaviour {

    [SerializeField]
    private Coordinate coordinate;
    public Coordinate Coordinate
    {
        get
        {
            return coordinate;
        }
        set
        {
            coordinate = value;
            Globals.Grid.UpdateCoordinate(this, coordinate);
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
    private bool isUpdatingCoordinate = false;
    public bool IsUpdatingCoordinate
    {
        get
        {
            return isUpdatingCoordinate;
        }
    }

    void Awake()
    {
        CreateDot();
    }

    public void CreateDot()
    {
        if (Globals.DotPrefab == null)
        {
            Debug.LogError("No valid Dot Prefab");
            return;
        }

        gridDot = Instantiate(Globals.DotPrefab);
        gridDot.transform.parent = this.transform;
        gridDot.transform.localPosition = Vector3.zero;
        gridDot.Color = Globals.GetRandomColor();
        gridDot.GridBox = this;
    }

    public void Initialize(Coordinate c, Vector3 spawnLocation)
    {
        coordinate = c;
        this.transform.position = spawnLocation;
        this.name = string.Format("Gridbox: ({0}, {1})", c.X, c.Y);
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
