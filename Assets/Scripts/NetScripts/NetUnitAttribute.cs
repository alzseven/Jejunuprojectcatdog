using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetUnitAttribute : NetworkBehaviour {

    public int Cost;
    public int AttackRange;
    public int HpMAX;
    [SyncVar]
    public int HP;
    public int AttackPower;
    public float AttackTime;
    public float MoveSpeed;
    public NetObjectManager.NetObjectType ObjectType;

    /*public Material[] Mat;
    public Color[] col;

    [SyncVar]
    public float AlphaV;
    */
    // Use this for initialization
    void Start () {
        /*Renderer[] Rd = GetComponentsInChildren<Renderer>();
        for (int i = 0; i < Rd.Length; i++)
        {
            Mat[i] = Rd[i].material;
            col[i] = Mat[i].color;
        }*/
    }
    /*[ClientRpc]
	void AlphaChange(float a)
    {
        for (int i = 0; i < 4; i++)
        {
            col[i].a = a;
            Mat[i].color = col[i];
        }
    }*/
	// Update is called once per frame
	void Update () {
        //AlphaChange(AlphaV);
	}
}
