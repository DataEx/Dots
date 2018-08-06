using UnityEngine;

[RequireComponent(typeof(PooledObject))]
public class Chain : MonoBehaviour {

    // Dot where the chain is originating from
    [SerializeField]
    Dot originDot = null;
    // Dot where the chain is pointing to
    [SerializeField]
    Dot connectedDot = null;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private PooledObject pooledObject;

    // The distance between the sprite's edges divided by its transform's size 
    private const float SPRITE_SIZE_RATIO = 0.8333f;

    private void Awake()
    {
        pooledObject = GetComponent<PooledObject>();
        pooledObject.Pool = Globals.ChainObjectPool;
    }


    public void Initialize(Dot dot)
    {
        originDot = dot;

        if (dot != null)
            spriteRenderer.material = dot.Color.MaterialColor;
        else
            spriteRenderer.material = null;
    }

    // Set the connectedDot
    public void ConnectTo(Dot dot)
    {
        connectedDot = dot;
        UpdateTransform(dot.transform.position);
    }

    // Connects a dot with either another dot or the cursor's location
    public void UpdateTransform(Vector3 endPoint)
    {

        if (originDot == null)
            return;

        endPoint.z = originDot.transform.position.z;

        // Position self between both objects
        this.transform.position = (originDot.transform.position + endPoint) / 2f;

        // If the chain is placed between two points exactly above each other, LookAt() will provide the wrong orientation.
        // A minor adjustment is needed to correct this.
        if (endPoint.x == originDot.transform.position.x)
        {
            this.transform.position += Vector3.right * Globals.SMALL_NUMBER;
        }

        // Orient self appropriately
        this.transform.LookAt(originDot.transform);

        // Move in back of dots so that the chain will always be drawn behind of the dots
        this.transform.position += Vector3.forward;

        // Scale chain between two objs (z-component only)
        float objDistance = Vector3.Distance(originDot.transform.position, endPoint);
        Vector3 chainScale = this.transform.localScale;
        chainScale.z = objDistance * SPRITE_SIZE_RATIO;

        this.transform.localScale = chainScale;
    }


    public bool IsConnectedTo(Dot dot)
    {
        return dot == originDot || dot == connectedDot;
    }

    public void DestroyChain()
    {
        pooledObject.ReturnToPool();
    }
}
