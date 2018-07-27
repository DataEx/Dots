using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DotColor {

    [SerializeField]
    private Material materialColor;
    public Material MaterialColor
    {
        get {
            return materialColor;
        }

        set {
            materialColor = value;
        }
    }

    public bool Equals(DotColor otherDotColor)
    {
        return MaterialColor == otherDotColor.MaterialColor;
    }
}
