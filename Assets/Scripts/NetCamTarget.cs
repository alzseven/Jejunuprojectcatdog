using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetCamTarget : MonoBehaviour {

    [SerializeField] Transform target;
    [SerializeField] string targetTag = "Player";

    Transform mytransform;

	// Use this for initialization
	void Start () {
        mytransform = transform;
        
    }
	
	// Update is called once per frame
	void Update () {
        if (!HaveTarget)
            return;
        Vector3 toPos = target.position + new Vector3(42.5f, 30f, 0);
        if (toPos == new Vector3(42.5f, 30f, -110f))
        {
            toPos += new Vector3(0, 0, 30f);
        }
        if (toPos == new Vector3(42.5f, 30f, 110f))
        {
            toPos -= new Vector3(0, 0, 30f);
        }
        transform.position = toPos;
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
