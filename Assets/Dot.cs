using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Dot : MonoBehaviour {

    GridBox gridBox;
    public GridBox GridBox
    {
        get
        {
            return gridBox;
        }
        set
        {
            gridBox = value;
        }
    }

    public Coordinate Coordinate
    {
        get
        {
            if (gridBox == null)
                return null;
            return gridBox.Coordinate;
        }
    }

    private DotColor color;
    public DotColor Color
    {
        get
        {
            return color;
        }
        set
        {
            if (value == null)
                rendererComponent.material = null;
            else
                rendererComponent.material = value.MaterialColor;
            color = value;
        }
    }


    [SerializeField]
    Chain chain;
    public Chain Chain
    {
        get
        {
            return chain;
        }
        set
        {
            chain = value;
        }
    }

    private Renderer rendererComponent;

    private void Awake()
    {
        rendererComponent = this.GetComponent<Renderer>();
    }


    public void DestroyDot()
    {
        if (Chain != null)
            Chain.DestroyChain();
        Destroy(this.gameObject);
    }
}
