using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PressToSpawn : NetworkBehaviour
{

    public GameObject Cat;
    public Button button;

    /*[Command]
    void Cmd_CreateObj()
    {
        GameObject go = Instantiate(Cat) as GameObject;

        //go.GetComponent<NetworkIdentity>().AssignClientAuthority(go.GetComponent<NetworkIdentity>().connectionToClient);

        if (go != null && NOM != null)
        {
            Transform t = go.transform;
            t.parent = NOM.transform;
        }
        NetObjectManager.Instance.AddObject(NetObjectManager.NetObjectType.Player, go);
        NetworkServer.Spawn(go);
    }*/

    /*public override void OnStartLocalPlayer()
    {
        //I kept this part in, because I don't know if this is the function that sets isLocalPlayer to true, 
        //or this function triggers after isLocalPlayer is set to true.
        base.OnStartLocalPlayer();

        //On initialization, make the client (local client and remote clients) tell the server to call on an [ClientRpc] method.
    }
    
    //Test
    [Command]
    public void Cmd_CreateObjByTest()
    {

        /*if (isLocalPlayer)
        {
            Cmd_CreateObj();
        }*/
        //this.GetComponent<NetworkIdentity>().AssignClientAuthority(this.GetComponent<NetworkIdentity>().connectionToClient);
        //RpcLog();
    //}
    /*[ClientRpc]
    public void RpcLog()
    {
        //First, checks to see what type of recipient is receiving this message. If it's server, the output message should tell the user what the type is.
        Debug.Log("RPC: This is " + (this.isServer ? " Server" : " Client"));

        if (isLocalPlayer)
        {
            GameObject go = Instantiate(Cat) as GameObject;

            //go.GetComponent<NetworkIdentity>().AssignClientAuthority(go.GetComponent<NetworkIdentity>().connectionToClient);

            if (go != null && NOM != null)
            {
                Transform t = go.transform;
                t.parent = NOM.transform;
            }
            NetObjectManager.Instance.AddObject(NetObjectManager.NetObjectType.Player, go);
            NetworkServer.Spawn(go);
            //Server code
            //This is run for spawning new non-player objects. Since it is a server calling to all clients (local and remote), it needs to pass in a
            //NetworkConnection that connects from server to THAT PARTICULAR client, who is going to own client authority on the spawned object.

            //GameObject obj = MonoBehaviour.Instantiate(this.spawnPrefab) as GameObject;
            //NetworkServer.SpawnWithClientAuthority(obj, this.connectionToClient);
        }
        else
        {
            //Client code
            //I realized this hardly runs. Placed a log message here for completeness.
            Debug.Log("Testing.");
        }
    }*/

    // Use this for initialization
    void Start()
    {
        if (isLocalPlayer)
        {
            GetComponentInChildren<Canvas>().enabled = true;
            gameObject.GetComponent<Renderer>().material.color = Color.blue;
            gameObject.name = "Player";

            button = GameObject.FindGameObjectWithTag("Button").GetComponent<Button>();
            button.onClick.AddListener(Button1);
            ClientScene.RegisterPrefab(Cat);
            //NOM = GameObject.Find("NetObjects");
        }
        else
        {
            gameObject.tag = "Enemy";
            gameObject.name = "Remote Player";
        }
    }

    public void Button1()
    {
        GameObject go = Instantiate(Cat) as GameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

