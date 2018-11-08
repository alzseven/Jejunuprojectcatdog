using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
//using Extension;
//using Analytics;

namespace MultiPlayer
{
    
}
[System.Serializable]
public struct UnitProperties
{
    public bool isInitialized;
    public bool isCommanded;    //Ready
    public bool isMoving;   //Move, isCommanded ==false;
    public bool isAttacking;    //Attack
    public int currentHealth;
    public int maxHealth;
    public int teamFactionID;
    public int attackFactor;
    public float speedFactor;
    public float attackCooldownFactor;
    public float alphacolor;
    public Color teamColor;
    public Vector3 mouseTargetPosition;
    public GameObject targetUnit;
}

[System.Serializable]
public struct NewChanges
{
    public bool isInitialized;
    public bool isCommanded;    //Ready
    public bool isMoving;   //Move, isCommanded ==false;
    public bool isAttacking;    //Attack
    public int damage;
    public int teamFactionID;
    public float newMaxHealth;
    public float newCurrentHealth;
    public int newAttack;
    public float newSpeed;
    public float newAttackCooldown;
    public float alphavalue;
    public Vector3 mousePosition;
    public Color teamColor;
    public GameObject targetUnit;

    public NewChanges Clear()
    {
        isInitialized = false;
        isCommanded = false;    //Ready
        isMoving = false;   //Move, isCommanded ==false;
        isAttacking = false;
        damage = 0;
        newMaxHealth = 3;
        newCurrentHealth = 3;
        newAttack = 1;
        newSpeed = 1;
        newAttackCooldown = 1f;
        mousePosition = Vector3.one * -9999;
        teamColor = Color.gray;
        alphavalue = 1f;
        targetUnit = null;
        return this;
    }
}


[System.Serializable]
public class NewGameUnit : NetworkBehaviour
{
    [SyncVar(hook = "OnPropertiesChanged")]
    public UnitProperties properties;

    public delegate void UpdateProperties(NewChanges changes);
    public UpdateProperties updateProperties;
    public int HpMAX;
    [SerializeField] private int HP;
    public int AttackPower;
    [SerializeField] private float AttackDelay;
    [SerializeField] private float MoveSpeed;
    public int Cost;
    public Sprite sprite;

    [SerializeField]
    private float recoveryCounter;

    [SyncVar] public float attackCooldownCounter;
    [SerializeField]
    private float takeDamageCounter;
    [SerializeField]
    private bool isDead;

    /*public override void OnStartAuthority()
    {
        base.OnStartAuthority();
    }*/
    public void Start()
    {
        //base.OnStartAuthority();

        if (!this.properties.isInitialized)
        {
            this.properties.isInitialized = true;
            this.properties.maxHealth = HpMAX;
            this.properties.currentHealth = properties.maxHealth;
            this.properties.mouseTargetPosition = Vector3.zero;
            this.properties.isCommanded = true;
            properties.isMoving = false;
            properties.isAttacking = false;
            properties.attackFactor = AttackPower;
            properties.speedFactor = MoveSpeed;
            properties.attackCooldownFactor = AttackDelay;
            properties.alphacolor = 0.4f;
            this.properties.targetUnit = null;
        }
        this.updateProperties += NewProperty;
        this.attackCooldownCounter = 0f;
        this.isDead = false;

        //NOTE(Thompson): Changing the name here, so I can really get rid of (Clone).
        this.name = "NewGameUnit";
    }

    public void Update()
    {
        if (!this.hasAuthority)
        {
            return;
        }
        //NOTE(Thompson): Common update codes for authoritative and non-authoritative objects goes here.
        if (!properties.isInitialized)
        {
            //NOTE(Thompson): Rare chance the game unit is not initialized correctly.
            //Therefore the best way to fix this is to re-initialize the game unit again.
            //Not sure if this fixes that rare issue.
            this.properties.isInitialized = true;
            this.properties.maxHealth = HpMAX;
            this.properties.currentHealth = properties.maxHealth;
            this.properties.mouseTargetPosition = Vector3.zero;
            properties.attackFactor = AttackPower;
            properties.speedFactor = MoveSpeed;
            properties.attackCooldownFactor = AttackDelay;
            properties.alphacolor = 1f;
            this.properties.targetUnit = null;

            this.updateProperties += NewProperty;
            this.attackCooldownCounter = 0f;
            this.isDead = false;
            return;
        }
        

        /*if(gameObject.tag == "Player")
        {
            NewChanges changes = unit.CurrentProperty();
            changes.isCommanded = false;
            changes.isMoving = true;
            changes.alphavalue = 1;
            CmdUpdateUnitProperty(unit.gameObject, changes);
        }*/


        if (HandleStatus())
        {
            return;
        }
        HandleReady();
        HandleMovement();
        HandleAttacking();
    }

    public void NewProperty(NewChanges changes)
    {
        UnitProperties pro = new UnitProperties();
        pro = this.properties;
        pro.isInitialized = changes.isInitialized;
        pro.teamFactionID = changes.teamFactionID;
        pro.isAttacking = changes.isAttacking;
        if (changes.damage > 0)
        {
            pro.currentHealth -= changes.damage;
        }
        if (pro.mouseTargetPosition != changes.mousePosition)
        {
            pro.mouseTargetPosition = changes.mousePosition;
        }
        pro.isCommanded = changes.isCommanded;
        pro.isMoving = changes.isMoving;
        pro.maxHealth = (int)changes.newMaxHealth;
        pro.attackFactor = changes.newAttack;
        pro.speedFactor = changes.newSpeed;
        pro.targetUnit = changes.targetUnit;
        //pro.attackCooldownFactor = changes.newAttackCooldown;

        pro.alphacolor = changes.alphavalue;
        Renderer[] renderer = GetComponentsInChildren<Renderer>();
        foreach(Renderer R in renderer)
        {
            Color color = R.material.color;
            color.a = changes.alphavalue;
            R.material.color = color;
        }
        /*Material[] Mat = new Material[4];
        Color[] col = new Color[4];
        for (int i = 0; i < 4; i++)
        {
            //renderer[i].material.SetColor("_Basic Outline-Alpha", new Color(0, 0, 0, changes.alphavalue));
            Mat[i] = renderer[i].material; //Cannot use Setcolor function on arrays
            //col[i] = ;
            col[i] = new Color(Mat[i].color.r, Mat[i].color.g, Mat[i].color.b, changes.alphavalue);
            Mat[i].color = col[i];
        }*/


        OnPropertiesChanged(pro);
    }

    public NewChanges CurrentProperty()
    {
        NewChanges changes = new NewChanges().Clear();
        changes.isInitialized = this.properties.isInitialized;
        changes.isCommanded = this.properties.isCommanded;
        changes.isMoving = properties.isMoving;
        changes.isAttacking = this.properties.isAttacking;
        changes.newCurrentHealth = this.properties.currentHealth;
        changes.newMaxHealth = this.properties.maxHealth;
        changes.newAttack = this.properties.attackFactor;
        changes.newSpeed = this.properties.speedFactor;
        //changes.newAttackCooldown = this.properties.attackCooldownFactor;
        changes.mousePosition = this.properties.mouseTargetPosition;
        changes.targetUnit = this.properties.targetUnit;
        changes.damage = 0;
        changes.teamFactionID = this.properties.teamFactionID;
        changes.alphavalue = properties.alphacolor;
        changes.teamColor = this.properties.teamColor;
        return changes;
    }
    IEnumerator AttackCoolTime()
    {
        while (attackCooldownCounter > 0)
            yield return null;
    }

    public void OnPropertiesChanged(UnitProperties pro)
    {
        this.properties = pro;
    }

    public void CallCmdupdateProperty(NewChanges changes)
    {
        this.CmdUpdateProperty(this.gameObject, changes);
    }

    public void AlphaChange(float a)
    {
        Renderer[] renderer = GetComponentsInChildren<Renderer>();
        foreach (Renderer R in renderer)
        {
            Color color = R.material.color;
            color.a = a;
            R.material.color = color;
        }
        /*Material[] Mat = new Material[4];
        Color[] col = new Color[4];
        for (int i = 0; i < 4; i++)
        {
            //renderer[i].material.SetColor("_Basic Outline-Alpha", new Color(0, 0, 0, a));
            Mat[i] = renderer[i].material; //Cannot use Setcolor function on arrays
            //col[i] = Mat[i].color;
            col[i] = new Color(Mat[i].color.r, Mat[i].color.g, Mat[i].color.b, a);
            Mat[i].color = col[i];
        }*/
    }
    public float GetAlphaCol() //
    {
        return properties.alphacolor;
    }

    //*** ----------------------------   PRIVATE METHODS  -------------------------
    private void HandleReady()
    {
        if (properties.isCommanded)
        {
            CmdMouseMoving(gameObject, properties.mouseTargetPosition);
        }
    }
    private void HandleMovement()
    {
        if (this.properties.isMoving)
        {
            if (properties.targetUnit != null)
            {
                if (properties.targetUnit.gameObject.GetComponent<NewGameUnit>().properties.currentHealth > 0)
                {
                    //attackCooldownCounter = properties.attackCooldownFactor;
                    NewChanges changes = CurrentProperty();
                    changes.isMoving = false;
                    CmdUpdateProperty(gameObject, changes);
                }
                
            }
            else
            {
                CmdGoFront(gameObject, properties.speedFactor);
                NewChanges changes = CurrentProperty();
                changes.alphavalue = 1f;
                CmdUpdateProperty(gameObject, changes);
            }

        }
    }

    private void HandleAttacking()
    {
        //타겟을 가지고 있을때~~
        if (properties.targetUnit != null && !properties.isAttacking)// && attackCooldownCounter <= 0
        {
            if (properties.targetUnit.gameObject.GetComponent<NewGameUnit>().properties.currentHealth > 0)
            {
                NewChanges changes = CurrentProperty();
                changes.isAttacking = true;
                NewProperty(changes);
                attackCooldownCounter = properties.attackCooldownFactor;
                CmdStartCo();
                CmdBang(gameObject, properties.targetUnit, properties.attackFactor);
                if (properties.targetUnit.gameObject.GetComponent<NewGameUnit>().properties.currentHealth <= 0)
                {
                    Debug.Log("AfterFightTargetDead");
                    changes = CurrentProperty();
                    //changes.targetUnit = null;
                    changes.isMoving = true;
                    NewProperty(changes);
                }
            }
            //attackCooldownCounter = properties.attackCooldownFactor;//
            /*Debug.Log("Godlike");
            changes = CurrentProperty();
            changes.isAttacking = false;
            NewProperty(changes);*/
        }
        /*else if (attackCooldownCounter > 0f)
        {
            NewChanges changes = CurrentProperty();
            changes.isAttacking = true;
            //Debug.Log("ASDf");
            NewProperty(changes);
        }*/
    }

    private bool HandleStatus()
    {
        //Returns TRUE if the game unit is destroyed, preventing all other actions from executing.
        //Returns FALSE if game unit is alive.

        if (properties.currentHealth <= 0 && !isDead && !properties.isCommanded)// || properties.isMoving || properties.isAttacking)
        {
            isDead = true;
            CmdDestroy(gameObject);
            return true;
        }
        if (properties.isAttacking)
        {
            if (attackCooldownCounter > 0)
            {
                attackCooldownCounter -= Time.deltaTime;
            }
            else
            {
                NewChanges changes = CurrentProperty();
                changes.isAttacking = false;
                if (properties.targetUnit==null || properties.targetUnit.gameObject.GetComponent<NewGameUnit>().properties.currentHealth <= 0)
                {
                    Debug.Log("TargetisDeadwhileCoolDown");
                    changes.isMoving = true;
                }              
                NewProperty(changes);
            }
        }
        /*else if (!properties.isAttacking && properties.targetUnit == null &&!properties.isMoving && !properties.isCommanded)
        {
            NewChanges changes = CurrentProperty();
            changes.isMoving = true;
            NewProperty(changes);
        }*/
        return false;
    }

    //----------------------------  COMMANDS and CLIENTRPCS  ----------------------------
    [Command]
    public void CmdStartCo()
    {
        StartCoroutine(AttackCoolTime());
    }
    /*[ClientRpc]
    public void RpcStartCo()
    {

    }*/
    [Command]
    public void CmdDestroy(GameObject targetUnit)
    {
        if (targetUnit != null)
        {
            NewGameUnit unit = targetUnit.GetComponent<NewGameUnit>();
            if (unit != null && NetworkServer.FindLocalObject(unit.netId))
            {
                if (targetUnit != null)
                {
                    NewChanges changes = unit.CurrentProperty();
                    if (changes.targetUnit != null && changes.targetUnit.Equals(this.gameObject))
                    {
                        changes.targetUnit = null;
                        unit.NewProperty(changes);
                    }
                }

                NewAtkRange range = this.GetComponentInChildren<NewAtkRange>();
                if (range != null)
                {
                    range.parent = null;
                }
                NetworkServer.Destroy(this.gameObject);
            }
        }
    }

    [Command]
    public void CmdUpdateProperty(GameObject obj, NewChanges changes)
    {
        if (obj != null)
        {
            NewGameUnit unit = obj.GetComponent<NewGameUnit>();
            if (unit != null)
            {
                unit.NewProperty(changes);
            }
        }
    }

    [Command]
    public void CmdBang(GameObject AttackerUnit, GameObject VictimUnit, int damage)//
    {
        //Debug.Log("No1.");
        if (gameObject != null && properties.targetUnit != null)
        {
            //Debug.Log("No2.");
            NewGameUnit victimUnit = VictimUnit.GetComponent<NewGameUnit>();
            //Debug.Log("No3.");
            NewGameUnit attackerUnit = AttackerUnit.GetComponent<NewGameUnit>();
           // Debug.Log("No4.");
            if (!(NetworkServer.FindLocalObject(victimUnit.netId) || NetworkServer.FindLocalObject(attackerUnit.netId)))
            {
                //Debug.Log("No4-1.");
                return;
            }
            if (victimUnit != null && attackerUnit != null && NetworkServer.FindLocalObject(victimUnit.netId) != null && NetworkServer.FindLocalObject(attackerUnit.netId) != null)//!attackerUnit.properties.isAttacking) 
            {
                //Debug.Log("No5-1.");
                NewChanges changes = victimUnit.CurrentProperty();
                //Debug.Log("No5-2.");
                changes.damage = damage;
                //Debug.Log("No5-3.");
                victimUnit.NewProperty(changes);
                //Debug.Log("No5-4.");
                //victimUnit.CallCmdupdateProperty(changes);
                //RpcDamage(damage);
            }
        }
    }
    /*[ClientRpc]
    public void RpcDamage(int damage)
    {
        NewGameUnit victimUnit = properties.targetUnit.GetComponent<NewGameUnit>();
        NewChanges changes = victimUnit.CurrentProperty();
        changes.damage = damage;
        victimUnit.NewProperty(changes);
        victimUnit.CallCmdupdateProperty(changes);   
    }*/

    [Command]
    public void CmdMouseMoving(GameObject obj, Vector3 pos)
    {
        if (obj != null)
        {
            RpcMouseMoving(obj, pos);
        }
    }
    [ClientRpc] //?
    public void RpcMouseMoving(GameObject obj, Vector3 pos)
    {
        pos.y = transform.position.y;
        if (Mathf.Approximately(pos.x, 0) || Mathf.Approximately(pos.z, 0))
        {
            //좌표 보정
        }
        if (!Mathf.Approximately(pos.x, 0) || !Mathf.Approximately(pos.z, 0))
        {
            pos.x = pos.x - (pos.x % 5);
            pos.z = pos.z - (pos.z % 5);
            if (Mathf.Approximately(pos.x % 10, 0))
                pos.x += 5;
            if (Mathf.Approximately(pos.z % 10, 0))
                pos.z += 5;
            transform.position = pos;
        }
    }
    [Command]
    public void CmdGoFront(GameObject obj, float speed)
    {
        //Debug.Log("ASdf");
        if (obj != null)
        {
            RpcGoFront(speed);
        }
    }
    [ClientRpc]
    public void RpcGoFront(float speed)
    {
        transform.Translate(Vector3.forward.normalized * speed * Time.deltaTime);
    }
}
