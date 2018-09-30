using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Only4Ground : MonoBehaviour
{
    public Vector3 SpawnPos = Vector3.zero;
    /*Renderer Rd;
	// Use this for initialization
	void OntriggerEnter(){
        Rd = this.gameObject.GetComponentInChildren<Renderer>();

        Material Mat = Rd.material;

        Color matColor = Mat.color;

        matColor.a = 1f;
        matColor.r = 1f;
        matColor.g = 1f;

        Mat.color = matColor;
    }*/
    void Update()
    {
        SpawnPos=this.transform.position;
    }
}
