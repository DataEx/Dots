using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Dot : MonoBehaviour {

    private const int MAX_CHAINS = 2;

    private GridBox gridBox;
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
    private Chain[] chains;
    public Chain[] Chains
    {
        get
        {
            return chains;
        }
    }

    private bool setForDestruction = false;
    public bool SetForDestruction
    {
        get
        {
            return setForDestruction;
        }
        set
        {
            setForDestruction = value;
        }
    }



    private Renderer rendererComponent;

    private void Awake()
    {
        rendererComponent = this.GetComponent<Renderer>();

        chains = new Chain[MAX_CHAINS];
    }

    public void AddChain(Chain newChain)
    {
        for(int i = 0; i < chains.Length; i++)
        {
            if (chains[i] == null)
            {
                chains[i] = newChain;
                return;
            }
        }
    }

    public bool CanAddAnotherChain()
    {
        int chainCount = 0;
        for (int i = 0; i < chains.Length; i++)
        {
            if (chains[i] != null)
            {
                chainCount++;
            }
        }

        return chainCount < MAX_CHAINS;
    }

    public void RemoveChain(Dot dot)
    {
        for (int i = 0; i < chains.Length; i++)
        {
            if (chains[i] != null && chains[i].IsConnectedTo(dot))
            {
                chains[i].DestroyChain();
                chains[i] = null;
                return;
            }
        }
    }

    public void DestroyDot()
    {
        if (Chains != null)
        {
            for(int i = 0; i < chains.Length; i++)
            {
                if (chains[i] != null)
                {
                    chains[i].DestroyChain();
                    chains[i] = null;
                }
            }
        }
        setForDestruction = true;
    }

    public bool IsConnectedTo(Dot dot)
    {
        // Check if this dot's chains have a connection to the other dot
        for (int i = 0; i < chains.Length; i++)
        {
            Chain chain = Chains[i];
            if (chain == null)
                continue;
            if (chain.IsConnectedTo(dot))
                return true;
        }


        // Check if other dot's chains have a connection to the this dot
        for (int i = 0; i < dot.Chains.Length; i++)
        {
            Chain chain = dot.Chains[i];
            if (chain == null)
                continue;
            if (chain.IsConnectedTo(this))
                return true;
        }

        return false;


    }
}
