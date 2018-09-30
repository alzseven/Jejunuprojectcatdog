using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour {

    public float moveSpeed = 700f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float h = Input.GetAxis("Horizontal");

        if(transform.position.z < -80.0f)
        {
            Vector3 temp = new Vector3(transform.position.x,
                transform.position.y,
                -80.0f);
            transform.position = temp;
        } else if(transform.position.z > 60.0f)
        {
            Vector3 temp = new Vector3(transform.position.x,
                transform.position.y,
                60.0f);
            transform.position = temp;
        } else
        {
            transform.Translate(h * Time.deltaTime * moveSpeed * Vector3.right);
        }
    }
}
