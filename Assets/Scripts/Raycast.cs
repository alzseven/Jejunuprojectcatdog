using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raycast : MonoBehaviour {
    private void Start()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    }
}
