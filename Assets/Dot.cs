using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            this.GetComponent<Renderer>().material = value.MaterialColor;
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


    public void DestroyDot()
    {
        if (Chain != null)
            Chain.DestroyChain();
        if(this != null)
            Destroy(this.gameObject);
    }
}
