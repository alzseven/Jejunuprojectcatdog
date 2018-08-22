using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    private MouseCtrlManager MCtrl = null;
    public GameObject Unit1;
    private void Start()
    {
        MCtrl = gameObject.AddComponent<MouseCtrlManager>();
    }
    // Use this for initialization
    public void SpawnT () {
        Instantiate(Unit1, MCtrl.V3targPos, Quaternion.Euler(0,0,0));
	}

}
