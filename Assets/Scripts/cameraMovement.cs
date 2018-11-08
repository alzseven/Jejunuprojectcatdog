using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    [SerializeField] Transform target;
    [SerializeField] string targetTag = "NewSpawner";

    public float moveSpeed = 10f;
	// Use this for initialization
	void Start () {
        if (HaveTarget)
        {
            Vector3 toPos = target.position + new Vector3(42.5f, 30f, 0);
            if (toPos == new Vector3(42.5f, 30f, -70f))
            {
                toPos += new Vector3(0, 0, 30f);
            }
            if (toPos == new Vector3(42.5f, 30f, 70f))
            {
                toPos -= new Vector3(0, 0, 30f);
            }
            transform.position = toPos;
        }
        
    }
	
	// Update is called once per frame
	void Update () {
        float h = Input.GetAxis("Horizontal");

        if(transform.position.z < -40.0f)
        {
            Vector3 temp = new Vector3(transform.position.x,
                transform.position.y,
                -40.0f);
            transform.position = temp;
        } else if(transform.position.z > 40.0f)
        {
            Vector3 temp = new Vector3(transform.position.x,
                transform.position.y,
                40.0f);
            transform.position = temp;
        } else
        {
            transform.Translate(h * Time.deltaTime * moveSpeed * Vector3.right);
        }
    }

    bool HaveTarget
    {
        get
        {
            if (target)
            {
                if (!target.CompareTag(targetTag))
                    target = null;
            }
            if (!target)
            {
                GameObject temp = GameObject.FindGameObjectWithTag(targetTag);
                if (temp)
                {
                    target = temp.transform;
                    return true;
                }
                else
                    return false;
            }
            else
                return true;

        }
    }
}
