using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class SyncTransform : NetworkBehaviour {

    [SyncVar] Vector3 syncPos;
    [SerializeField] Transform mytransform;
    [SerializeField] float lerpRate = 15f;
	
	// Update is called once per frame
	void Update () {
        if (isServer)
        {
            RpcProvidePositionToClient(mytransform.position, mytransform.rotation);
        }
	}
    void LerpPosition()
    {

    }
    [ClientRpc]
    void RpcProvidePositionToClient(Vector3 pos, Quaternion rot)
    {
        if (!isServer)
        {
            mytransform.position = pos;
            mytransform.rotation = rot;
        }
    }
}
