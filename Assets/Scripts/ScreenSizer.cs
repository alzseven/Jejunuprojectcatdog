using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSizer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyUp(KeyCode.Space))
        {
            Screen.SetResolution(1920, 1080, false);
        }
        if (Input.GetKeyUp(KeyCode.Keypad9))
        {
            Screen.SetResolution(800, 600, false);
        }
    }
}
