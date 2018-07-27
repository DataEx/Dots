using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour {


    public static Globals instance;


    [SerializeField]
    GameObject dotPrefab;
    [SerializeField]
    GameObject chainPrefab;
    [SerializeField]
    GameObject gridBoxPrefab; // TODO: Instead of GameObject, should it be GridBox gridBoxPrefab?

    [SerializeField]
    float gridBoxZOffset = 1.0f;

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
    // Where (0,0) will be spawned
    Vector3 gridOrigin = Vector3.zero;
    public static Vector3 GridOrigin
    {
        get
        {
            return instance.gridOrigin;
        }
    }


    [SerializeField]
    DotColor[] colors;

    [SerializeField]
    int gridWidth;

    [SerializeField]
    int gridHeight;

    [SerializeField]
    Grid grid;
    public static Grid Grid
    {
        get {
            return instance.grid;
        }
    }

    public static GameObject DotPrefab
    {
        get
        {
            return instance.dotPrefab;
        }
    }

    public static GameObject ChainPrefab
    {
        get
        {
            return instance.chainPrefab;
        }
    }

    public static GameObject GridBoxPrefab
    {
        get
        {
            return instance.gridBoxPrefab;
        }
    }

    public static DotColor[] Colors
    {
        get
        {
            return instance.colors;
        }
    }

    public static float GridBoxZOffset
    {
        get
        {
            return instance.gridBoxZOffset;
        }
    }

    public static int GridWidth
    {
        get
        {
            return instance.gridWidth;
        }
    }

    public static int GridHeight
    {
        get
        {
            return instance.gridHeight;
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
