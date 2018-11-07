using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Collections;
using System;
//using Common;
//using Analytics;

namespace MultiPlayer
{
    
}
public struct NewUnitStruct
{
    public GameObject unit;

    public NewUnitStruct(GameObject unit)
    {
        this.unit = unit;
    }
}
public class UnitsSyncList : SyncListStruct<NewUnitStruct>
{
}
public class NewSpawner : NetworkBehaviour
{
    [SyncVar] public Color TeamColor;
    [SyncVar] public string HpBarName;
    public string[] DeckList = new string[5];
    public GameObject[] Card = new GameObject[5];
    //public GameObject[] CatCardList = new GameObject[10];

    [SyncVar] public bool cat;//?

    GameTimer GT;
    public GameObject newGameUnitPrefab; //base
    
    //unit list 

    public GameObject CardUI;
    public GameObject Underbar;

    public float source;
    public NetworkConnection owner;
    public UnitsSyncList unitList = new UnitsSyncList();
    public GameObject starterObjects;
    public static NewSpawner Instance;
    public bool isGameStart = false;
    public bool isPaused;
    public int TeamID;
    private Vector3 initialClick;
    private Vector3 screenPoint;
    private NewChanges changes;
    private bool isUnitListEmpty;
    public static int colorCode = 0;
    public static int MAX_UNIT_LIMIT = 16;
    public Quaternion vec = new Quaternion(0, 0, 0, 0);
    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        if (isLocalPlayer)
        {

            //gameObject.GetComponentInChildren<Renderer>().material.color = Color.blue;
            gameObject.name = HpBarName;

            for(int i = 0; i < 5; i++)
            {
                Card[i] = Resources.Load(DeckList[i]) as GameObject;
                ClientScene.RegisterPrefab(Card[i]);
            }
            // NOM = GameObject.Find("NetObjects");
        }
        else
        {
            gameObject.tag = "Enemy";
            gameObject.name = "Remote Player";
        }
        //This is used to obtain inactive start locations. Start locations can randomly
        //be set to inactive, so I need a way to obtain these inactive game objects.
        GT = GetComponent<GameTimer>();
        if (transform.position.z < 0)
        {
            vec = new Quaternion(0, 0, 0, 0);
        }
        else if (transform.position.z > 0)
        {
            vec = new Quaternion(0, 180, 0, 0);
        }
        starterObjects = GameObject.FindGameObjectWithTag("Respawn"); //
        if (starterObjects == null)
        {
            Debug.LogError("Cannot find starter object in scene.");
        }
        if (!hasAuthority)
        {
            return;
        }
        NetworkIdentity spawnerIdentity = GetComponent<NetworkIdentity>();
        if (!spawnerIdentity.localPlayerAuthority)
        {
            spawnerIdentity.localPlayerAuthority = true;
        }
        owner = isServer ? spawnerIdentity.connectionToClient : spawnerIdentity.connectionToServer;
        isPaused = false;
        initialClick = Vector3.one * -9999f;
        screenPoint = initialClick;
        isGameStart = false;
        isUnitListEmpty = true;
        //NOTE(Thompson): NewSpawner needs to keep track of where the first game unit is spawned at. This is set in CmdInitialize().
        changes.Clear();
        //?
        //CmdConCheck();
        //, "Prefabs/Base");
        CmdInitialize(gameObject);

    }
    IEnumerator WaitForReady()
    {
        while (!connectionToClient.isReady)
        {
            yield return new WaitForSeconds(0.25f);
        }
        //, "Prefabs/Base");
    }
    [Command]
    public void CmdConCheck()
    {
        if (connectionToClient.isReady)
        {

            CmdInitialize(gameObject);
        }
        else
        {
            StartCoroutine("WaitForReady");
            CmdInitialize(gameObject);
        }

    }
    [Command]
    public void CmdSetAuth()
    {
        NetworkIdentity spawnerIdentity = GetComponent<NetworkIdentity>();
        if (!spawnerIdentity.localPlayerAuthority)
        {
            spawnerIdentity.localPlayerAuthority = true;
        }
        owner = isServer ? spawnerIdentity.connectionToClient : spawnerIdentity.connectionToServer;
        /*
        var iObject = NetworkServer.FindLocalObject(objectId);
        var networkIdentity = iObject.GetComponent<NetworkIdentity>();
        var otherOwner = networkIdentity.clientAuthorityOwner;

        if (otherOwner == player.connectionToClient)
        {
            return;
        }
        else
        {
            if (otherOwner != null)
            {
                networkIdentity.RemoveClientAuthority(otherOwner);
            }
            networkIdentity.AssignClientAuthority(player.connectionToClient);
        }
        */
    }

    [Command]
    public void CmdSetReadyFlag(bool value)
    {
        if (value)
        {
            RpcSetReadyFlag();
        }
    }

    [ClientRpc]
    public void RpcSetReadyFlag()
    {
        isGameStart = true;
    }

    [Command]
    public void CmdAddUnit(GameObject obj, GameObject spawner)
    {
        NewSpawner newSpawner = spawner.GetComponent<NewSpawner>();
        if (newSpawner != null)
        {
            newSpawner.unitList.Add(new NewUnitStruct(obj));
            newSpawner.isUnitListEmpty = false;
        }
    }

    [ClientRpc]
    public void RpcAdd(GameObject obj, GameObject spawner)
    {
        //CmdAddUnit(obj, spawner);
        if (hasAuthority)
        {
            CmdAddUnit(obj, spawner);
        }
    }

    /// <summary>
    /// instantiate+spawnwithauthor
    /// </summary>
    /// <param name="spawner"></param>
    [Command]
    public void CmdInitialize(GameObject spawner)//, string whatpath)
    {
        NetworkIdentity spawnerID = spawner.GetComponent<NetworkIdentity>();
        //Only the server choose what color values to use. Client values do not matter.
        //should fix
        int colorValue = colorCode;
        colorCode = (++colorCode) % 3;
        Color color;
        switch (colorValue)
        {
            default:
                color = Color.gray;
                break;
            case 0:
                color = Color.yellow;
                break;
            case 1:
                color = Color.blue;
                break;
            case 2:
                color = Color.green;
                break;
        }

        GameObject gameUnit = Instantiate(newGameUnitPrefab);
        //ClientScene.RegisterPrefab(gameUnit);
        gameUnit.name = gameUnit.name.Substring(0, gameUnit.name.Length - "(Clone)".Length);
        gameUnit.transform.SetParent(transform);
        gameUnit.transform.position = transform.position;
        NewGameUnit b = gameUnit.GetComponent<NewGameUnit>();
        NewChanges changes = b.CurrentProperty();
        if (!changes.isInitialized)
        {
            changes.isInitialized = false;
            changes.teamFactionID = (int)(UnityEngine.Random.value * 100f); //This is never to be changed.
        }
        TeamID = changes.teamFactionID;
        b.NewProperty(changes);
        //NetworkServer.SpawnWithClientAuthority(b.gameObject, spawnerID.clientAuthorityOwner);
        //ClientScene.RegisterPrefab(b.gameObject);
        NetworkServer.Spawn(b.gameObject);
        b.gameObject.GetComponent<NetworkIdentity>().AssignClientAuthority(spawnerID.clientAuthorityOwner);
        RpcAdd(gameUnit, spawner);
        RpcFilter(b.netId, spawnerID.netId);
        /*if (connectionToClient.isReady)
        {

        }
        else
        {
            connectionToClient.RegisterHandler(MsgType.Ready, OnReady);
        }
        //newGameUnitPrefab = b.gameObject;*/
    }

    void OnReady(NetworkMessage netMsg)
    {
        NetworkIdentity spawnerID = this.GetComponent<NetworkIdentity>();
        GameObject spawner = this.gameObject;

        GameObject gameUnit = Instantiate(newGameUnitPrefab);
        gameUnit.name = gameUnit.name.Substring(0, gameUnit.name.Length - "(Clone)".Length);
        gameUnit.transform.SetParent(transform);
        gameUnit.transform.position = transform.position;
        NewGameUnit b = gameUnit.GetComponent<NewGameUnit>();
        NewChanges changes = b.CurrentProperty();
        if (!changes.isInitialized)
        {
            changes.isInitialized = false;
            changes.teamFactionID = (int)(UnityEngine.Random.value * 100f); //This is never to be changed.
        }
        TeamID = changes.teamFactionID;
        b.NewProperty(changes);
        NetworkServer.SpawnWithClientAuthority(b.gameObject, spawnerID.clientAuthorityOwner);
        RpcAdd(gameUnit, spawner);
        RpcFilter(b.netId, spawnerID.netId);
    }

    [ClientRpc]
    public void RpcFilter(NetworkInstanceId unitID, NetworkInstanceId spawnerID)
    {
        //NOTE(Thompson): This organizes the Hierarchy in Unity, so player units are children of player owned NewSpawners.
        GameObject obj = ClientScene.FindLocalObject(unitID);
        GameObject spawnerObject = ClientScene.FindLocalObject(spawnerID);
        obj.transform.SetParent(spawnerObject.transform);
        NewGameUnit unit = obj.GetComponent<NewGameUnit>();
        if (unit.tag == "Player")
        {
            changes = unit.CurrentProperty();
            changes.isCommanded = false;
            changes.isMoving = true;
            CmdUpdateUnitProperty(unit.gameObject, changes);
        }
        if (unit.hasAuthority)
        {
            //Camera Setup
            unit.AlphaChange(unit.properties.alphacolor); //NOTE(Thompson): This has to do with triggering the SyncVar's hook.
        }

        NewGameUnit[] units = FindObjectsOfType<NewGameUnit>();
        NewSpawner[] spawners = FindObjectsOfType<NewSpawner>();
        for (int i = 0; i < units.Length; i++)
        {
            if (!units[i].hasAuthority)
            {
                for (int j = 0; j < spawners.Length; j++)
                {
                    if (!spawners[j].hasAuthority)
                    {
                        units[i].transform.SetParent(spawners[j].transform);
                        unit.AlphaChange(unit.properties.alphacolor); //NOTE(Thompson): This has to do with triggering the SyncVar's hook.
                    }
                }
            }
        }
    }

    [ClientRpc]
    public void RpcOrganizeUnit(GameObject owner, GameObject split)
    {
        split.transform.SetParent(owner.transform.parent);
    }

    public void Update()
    {
        //NOTE(Thompson): Common codes for server and clients go here.
        //Common codes end here.

        //NOTE(Thompson): Codes needed to run for non-authority objects go here.
        if (!hasAuthority)
        {
            return;
        }
        /*if (!isUnitListEmpty)
        {
            return;
        }*/

        HandleSelection();
        HandleButtons();
    }

    [Command]
    public void CmdDestroy(GameObject obj)
    {
        NetworkServer.Destroy(obj);
    }

    [Command]
    public void CmdSplitSpawn(GameObject spawn,GameObject owner)//, NetworkIdentity ownerIdentity)
    {
        //NetworkIdentity spawnerID = this.GetComponent<NetworkIdentity>();

        GameObject split = Instantiate(spawn, gameObject.transform.position, vec);

        split.name = "NewGameUnit";
        split.transform.SetParent(transform);
        split.transform.position = transform.position;
        NewGameUnit unit = split.GetComponent<NewGameUnit>();
        NewChanges changes = unit.CurrentProperty();
        changes.teamFactionID = TeamID;
        unit.NewProperty(changes);
        NetworkServer.SpawnWithClientAuthority(unit.gameObject, owner.gameObject.GetComponent<NetworkIdentity>().clientAuthorityOwner);
        //NetworkServer.Spawn(unit.gameObject);
        //unit.gameObject.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);//ownerIdentity.clientAuthorityOwner);
        RpcAddSplit(gameObject, unit.gameObject, changes);
        RpcAdd(unit.gameObject, this.gameObject);
        RpcFilter(unit.netId, gameObject.GetComponent<NetworkIdentity>().netId);
    }
    [ClientRpc]
    public void RpcAddSplit(GameObject owner, GameObject split, NewChanges changes)
    {
        if (owner != null && split != null)
        {
            NewGameUnit splitUnit = split.GetComponent<NewGameUnit>();
            if (splitUnit != null)
            {
                //splitUnit.NewProperty(changes);
                splitUnit.AlphaChange(splitUnit.properties.alphacolor);
            }
            else
            {
                Debug.LogWarning("SplitUnit does not exist.");
            }
            this.unitList.Add(new NewUnitStruct(split));
        }
        else
        {
            string value1 = (owner == null) ? " Owner is null." : "";
            string value2 = (split == null) ? " Split is null." : "";
            Debug.LogWarning(value1 + value2);
        }
    }
    [Command]
    public void CmdUpdateUnitProperty(GameObject unit, NewChanges changes)
    {
        if (unit != null)
        {
            NewGameUnit gameUnit = unit.GetComponent<NewGameUnit>();
            if (gameUnit != null)
            {
                gameUnit.NewProperty(changes);
            }
        }
    }
    [Command]
    public void CmdRemoveUnitList(GameObject obj)
    {
        if (obj != null)
        {
            this.unitList.Remove(new NewUnitStruct(obj));
        }
        else
        {
            for (int i = this.unitList.Count - 1; i >= 0; i--)
            {
                if (this.unitList[i].unit == null)
                {
                    this.unitList.RemoveAt(i);
                }
            }
        }
    }
    //-----------   Private class methods may all need refactoring   --------------------
    private void HandleButtons()    //Try Button Pressing
    {

        if (Input.GetKeyUp(KeyCode.Z))//
        {
            if (Card[0].GetComponent<NewGameUnit>().Cost < source)
            {
                source -= Card[0].GetComponent<NewGameUnit>().Cost;
                CmdSplitSpawn(Card[0],gameObject);
            }
            //, gameObject.GetComponent<NetworkIdentity>());
            /*foreach(NewUnitStruct temp in unitList)
            {
                NewGameUnit newUnit = temp.unit.GetComponent<NewGameUnit>();
                if (newUnit.gameObject.tag == "Player")
                {

                }
            }*/
            //NewGameUnit unit = unitList[0].unit.GetComponent<NewGameUnit>();
            //GameObject Base = GameObject.FindGameObjectWithTag("Player");

        }
        if (Input.GetKeyUp(KeyCode.X))//
        {
            if (Card[1].GetComponent<NewGameUnit>().Cost < source)
            {
                source -= Card[1].GetComponent<NewGameUnit>().Cost;
                CmdSplitSpawn(Card[1],gameObject);
            }

        }
        if (Input.GetKeyUp(KeyCode.C))//
        {
            if (Card[2].GetComponent<NewGameUnit>().Cost < source)
            {
                source -= Card[2].GetComponent<NewGameUnit>().Cost;
                CmdSplitSpawn(Card[2],gameObject);
            }

        }
        if (Input.GetKeyUp(KeyCode.V))//
        {
            if (Card[3].GetComponent<NewGameUnit>().Cost < source)
            {
                source -= Card[3].GetComponent<NewGameUnit>().Cost;
                CmdSplitSpawn(Card[3],gameObject);
            }

        }
        if (Input.GetKeyUp(KeyCode.B))//
        {
            if (Card[4].GetComponent<NewGameUnit>().Cost < source)
            {
                source -= Card[4].GetComponent<NewGameUnit>().Cost;
                CmdSplitSpawn(Card[4],gameObject);
            }

        }

    }
    private void HandleSelection()
    {
        for (int i = this.unitList.Count - 1; i >= 0; i--)
        {
            NewUnitStruct temp = this.unitList[i];
            GameObject obj = temp.unit.gameObject;
            if (obj == null || obj.tag == "Player")
            {
                unitList.Remove(temp);
                continue;
            }
            NewGameUnit unit = obj.GetComponent<NewGameUnit>();
            if (unit.properties.isCommanded)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray.origin, ray.direction, out hit, 1000f) && hit.collider.tag == "ground")
                {
                    changes = unit.CurrentProperty();
                    changes.mousePosition = hit.point;
                    CmdUpdateUnitProperty(unit.gameObject, changes);
                }
                if (Input.GetMouseButtonUp(0))
                {
                    changes = unit.CurrentProperty();
                    changes.alphavalue = 1f;
                    changes.isCommanded = false;
                    changes.isMoving = true;
                    CmdUpdateUnitProperty(unit.gameObject, changes);
                    break;
                }
            }
        }
    }
    


}
