using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour {


    public static Globals instance;

    [SerializeField]
    Dot dotPrefab;
    public static Dot DotPrefab
    {
        get
        {
            return instance.dotPrefab;
        }
    }

    [SerializeField]
    Chain chainPrefab;
    public static Chain ChainPrefab
    {
        get
        {
            return instance.chainPrefab;
        }
    }

    [SerializeField]
    GridBox gridBoxPrefab;
    public static GridBox GridBoxPrefab
    {
        get
        {
            return instance.gridBoxPrefab;
        }
    }
    
    // How far Dot is away GridBox 
    [SerializeField]
    float gridBoxZOffset = 1.0f;
    public static float GridBoxZOffset
    {
        get
        {
            return instance.gridBoxZOffset;
        }
    }

    // Speed at which dots fall
    [SerializeField]
    float fallingSpeed = 0.1f;
    public static float FallingSpeed
    {
        get
        {
            return instance.fallingSpeed;
        }
    }

    [SerializeField]
    // Where the grid's coordinate (0,0) will be spawned
    Vector3 gridOrigin = Vector3.zero;
    public static Vector3 GridOrigin
    {
        get
        {
            return instance.gridOrigin;
        }
    }


    // The different colors that will be used when generated dots
    [SerializeField]
    DotColor[] colors;
    public static DotColor[] Colors
    {
        get
        {
            return instance.colors;
        }
    }

    [SerializeField]
    int gridWidth;
    public static int GridWidth
    {
        get
        {
            return instance.gridWidth;
        }
    }

    [SerializeField]
    int gridHeight;
    public static int GridHeight
    {
        get
        {
            return instance.gridHeight;
        }
    }

    [SerializeField]
    GridController grid;
    public static GridController Grid
    {
        get {
            return instance.grid;
        }
    }

    public static DotColor GetRandomColor()
    {
        int colorCount = Colors.Length;
        int randomIndex = Random.Range(0, colorCount);
        return Colors[randomIndex];
    }

    void Awake()
    {
        instance = this;
    }
}
