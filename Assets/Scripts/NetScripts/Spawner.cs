using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.UI;

public class Spawner : NetworkBehaviour
{
    public GameObject spawnPrefab;
    public GameObject Cat;
    public Button button;


    public override void OnStartLocalPlayer()
    {
        //I kept this part in, because I don't know if this is the function that sets isLocalPlayer to true, 
        //or this function triggers after isLocalPlayer is set to true.
        base.OnStartLocalPlayer();

        //On initialization, make the client (local client and remote clients) tell the server to call on an [ClientRpc] method.
        //CmdCall();
    }
    void Start()
    {
        if (isLocalPlayer)
        {
            
            gameObject.GetComponent<Renderer>().material.color = Color.blue;
            gameObject.name = "Player";

            
            ClientScene.RegisterPrefab(Cat);
           // NOM = GameObject.Find("NetObjects");
        }
        else
        {
            gameObject.tag = "Enemy";
            gameObject.name = "Remote Player";
        }
    }
    void Update()
    {
        if (isLocalPlayer)
        {
            GetComponentInChildren<Canvas>().enabled = true;
            if (Input.GetKey(KeyCode.Keypad1))
            {
                CmdCall();
            }
        }
            
        //button = GameObject.FindGameObjectWithTag("Button").GetComponent<Button>();
        //button.onClick.AddListener(Button1);
        
    }
    /*public void Button1()
    {
        CmdCall();
    }*/
    [Command]
    public void CmdCall()
    {
        //Calling [ClientRpc] on the server.
        RpcLog();
    }

    [ClientRpc]
    public void RpcLog()
    {
        //First, checks to see what type of recipient is receiving this message. If it's server, the output message should tell the user what the type is.
        Debug.Log("RPC: This is " + (this.isServer ? " Server" : " Client"));

        if (isServer)
        {
            //Server code
            //This is run for spawning new non-player objects. Since it is a server calling to all clients (local and remote), it needs to pass in a
            //NetworkConnection that connects from server to THAT PARTICULAR client, who is going to own client authority on the spawned object.
            GameObject go = MonoBehaviour.Instantiate(this.Cat) as GameObject;
            NetworkServer.SpawnWithClientAuthority(go, this.connectionToClient);
        }
        else
        {
            //Client code
            //I realized this hardly runs. Placed a log message here for completeness.
            Debug.Log("Testing.");
        }
    }
}
