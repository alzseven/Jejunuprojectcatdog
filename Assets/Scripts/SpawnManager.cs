using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    public GameObject StartPoint;
    //private MouseCtrlManager MCtrl = null;
    public GameObject Unit1;
    private string OriName;
    private string ClName;
    private float Count = 0f;
    private void Start()
    {
        //MCtrl = gameObject.AddComponent<MouseCtrlManager>();
    }
    // Use this for initialization
    public void SpawnT () {
        var Ground = StartPoint.GetComponent<Only4Ground>();
        GameObject Clone1 = Instantiate(Unit1, Ground.SpawnPos/*MCtrl.V3targPos*/, Quaternion.Euler(0,0,0));
        OriName = Unit1.name;
        ClName = "Player1_"+OriName + Count;
        Clone1.name = ClName;
        Count += 1f;
 	}

}
