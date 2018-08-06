using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PooledObject))]
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

    private PooledObject pooledObject;

    void OnEnable()
    {
        CreateDot();
        if (pooledObject == null)
        {
            pooledObject = GetComponent<PooledObject>();
            pooledObject.Pool = Globals.GridBoxObjectPool;
        }

    }

    public void CreateDot()
    {
        if (Globals.DotPrefab == null)
        {
            Debug.LogError("No valid Dot Prefab");
            return;
        }

        if (gridDot == null)
        {
            gridDot = Instantiate(Globals.DotPrefab);
            gridDot.transform.parent = this.transform;
            gridDot.transform.localPosition = Vector3.zero;
            gridDot.GridBox = this;
        }

        gridDot.SetForDestruction = false;
        gridDot.Color = Globals.GetRandomColor();
    }

    public void Initialize(Coordinate c, Vector3 spawnLocation)
    {
        Coordinate = c;
        this.transform.position = spawnLocation;
        this.name = string.Format("Gridbox: ({0}, {1})", c.X, c.Y);
        if(gridDot != null)
            gridDot.name = string.Format("Dot: ({0}, {1})", c.X, c.Y);

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

    public void DestroyGridBox()
    {
        pooledObject.ReturnToPool();
    }
}
