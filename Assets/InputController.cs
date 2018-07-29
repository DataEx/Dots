using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {

        PlayerInput.isClicking = Input.GetMouseButton(0);
        PlayerInput.mousePosition = Input.mousePosition;
    }
}
