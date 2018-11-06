using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class NetGetMouse : NetworkBehaviour
{
    //private static NetGetMouse instance_ = null;
    //public static NetGetMouse Instance { get { return instance_; } private set { } }

    private bool isPressedMouseButton = false;
    //private Vector3 getTargetPos = Vector3.zero;
    public bool IsPressedMouseButton { get { return isPressedMouseButton; } }
    //public Vector3 GetTargetPos { get { return getTargetPos; } }
    //[SyncVar]
    public bool Online = false;
    public Vector3 getTargetPos = Vector3.zero;

    //Camera mainCam;

    /*public bool Online()
    {
        
    }*/
        
    private void Awake()
    {
        //instance_ = this;
        //mainCam = GetComponent<Camera>();
    }
    void Update()
    {
        
        MouseRay();
        if (Input.GetMouseButtonDown(0))
        {
            isPressedMouseButton = true;
        }
        /*if (isLocalPlayer)
        {
            MouseRay();
            if (Input.GetMouseButtonDown(0))
            {
                isPressedMouseButton = true;
            }
        }*/

    }

    //[ClientRpc]
    void MouseRay()
    {
        isPressedMouseButton = false;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 1000f) && hit.collider.tag == "ground")
        {
            //Cmd_TargetPos(hit.point);
            getTargetPos = hit.point;
            //Debug.Log(hit.transform.name);
        }
    }
    [ClientRpc]
    void Rpc_TargetPos(Vector3 pos)
    {
        getTargetPos = pos;
    }
    [Command]
    void Cmd_TargetPos(Vector3 pos)
    {
        Rpc_TargetPos(pos);
    }
}
