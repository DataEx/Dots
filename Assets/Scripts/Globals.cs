using UnityEngine;

public class Globals : MonoBehaviour {

    public static Globals instance;

    [SerializeField]
    private Dot dotPrefab;
    public static Dot DotPrefab
    {
        get
        {
            return instance.dotPrefab;
        }
    }

    [SerializeField]
    private Chain chainPrefab;
    public static Chain ChainPrefab
    {
        get
        {
            return instance.chainPrefab;
        }
    }

    [SerializeField]
    private GridBox gridBoxPrefab;
    public static GridBox GridBoxPrefab
    {
        get
        {
            return instance.gridBoxPrefab;
        }
    }


    [SerializeField]
    private ObjectPool chainObjectPool;
    public static ObjectPool ChainObjectPool
    {
        get
        {
            return instance.chainObjectPool;
        }
    }

    [SerializeField]
    private ObjectPool gridBoxObjectPool;
    public static ObjectPool GridBoxObjectPool
    {
        get
        {
            return instance.gridBoxObjectPool;
        }
    }

    [SerializeField] [Tooltip("Speed at which dots fall. Must be greater than 0.")]
    private float fallingSpeed = 0.1f;
    public static float FallingSpeed
    {
        get
        {
            return instance.fallingSpeed;
        }
    }

    [SerializeField]
    private GridController grid;
    public static GridController Grid
    {
        get
        {
            return instance.grid;
        }
    }

    [SerializeField] [Tooltip("Where the Grid's coordinate (0,0) will be spawned.")]
    private Vector3 gridOrigin = Vector3.zero;
    public static Vector3 GridOrigin
    {
        get
        {
            return instance.gridOrigin;
        }
    }

    [SerializeField]
    private int gridWidth;
    public static int GridWidth
    {
        get
        {
            return instance.gridWidth;
        }
    }

    [SerializeField]
    private int gridHeight;
    public static int GridHeight
    {
        get
        {
            return instance.gridHeight;
        }
    }

    [SerializeField] [Tooltip("The different colors that will be used when generated dots")]
    private DotColor[] colors;
    public static DotColor[] Colors
    {
        get
        {
            return instance.colors;
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
