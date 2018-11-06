using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PreS : NetworkBehaviour
{
    //private static NetGetMouse instance_ = null;
    //public static NetGetMouse Instance { get { return instance_; } private set { } }

    private bool isPressedMouseButton = false;
    //private Vector3 getTargetPos = Vector3.zero;
    public bool IsPressedMouseButton { get { return isPressedMouseButton; } }
    //public Vector3 GetTargetPos { get { return getTargetPos; } }
    //[SyncVar]

    public GameObject Cat;
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
            CmdCall(getTargetPos);
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
            Debug.Log(hit.transform.name);
        }
    }
    /*[ClientRpc]
    void Rpc_TargetPos(Vector3 pos)
    {
        getTargetPos = pos;
    }
    [Command]
    void Cmd_TargetPos(Vector3 pos)
    {
        Rpc_TargetPos(pos);
    }*/
    [Command]
    public void CmdCall(Vector3 pos)
    {
        //Calling [ClientRpc] on the server.
        RpcLog(pos);
    }

    [ClientRpc]
    public void RpcLog(Vector3 pos)
    {
        //First, checks to see what type of recipient is receiving this message. If it's server, the output message should tell the user what the type is.
        Debug.Log("RPC: This is " + (this.isServer ? " Server" : " Client"));

        if (isServer)
        {
            //Server code
            //This is run for spawning new non-player objects. Since it is a server calling to all clients (local and remote), it needs to pass in a
            //NetworkConnection that connects from server to THAT PARTICULAR client, who is going to own client authority on the spawned object.
            GameObject go = Instantiate(Cat,pos,new Quaternion (0,0,0,0)) as GameObject;
            NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
        }
        else
        {
            //Client code
            //I realized this hardly runs. Placed a log message here for completeness.
            Debug.Log("Testing.");
        }
    }
}

