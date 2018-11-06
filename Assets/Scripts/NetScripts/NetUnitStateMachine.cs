using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetUnitStateMachine : NetworkBehaviour
{


    Animator anime;

    Material[] Mat;
    Color[] col;
    private Vector3 vecPos = Vector3.zero;

    NetGetMouse NG;
    public GameObject Base;
    public GameObject Target { get; set; } //대상
    public Vector3 TargetPosition { get; set; } //대상의 위치
    //float a = 1f;

    public enum State
    {
        INITIALIZE,
        READY,
        MOVE,
        ATTACK,
        DEAD,
    };

    private State currentState_ = State.INITIALIZE;
    private float nextAttackCooltime_ = 0.0f;
    // Use this for initialization
    void Awake()
    {
        //gameObject.GetComponent<NetworkIdentity>().AssignClientAuthority(gameObject.GetComponent<NetworkIdentity>().connectionToClient);
        Target = null;
        SetState(State.READY);
        anime = GetComponent<Animator>();
        Base = GameObject.FindGameObjectWithTag("Player");
        NG = Base.GetComponent<NetGetMouse>();
    }

    ///<summary>
    /// 상태를 변경. 같은상태로는 바뀌지 않음
    /// </summary>
    /// <param name ="newState"></param>
	public void SetState(State newState)
    {
        if (newState == currentState_)
            return;
        if (currentState_ == State.DEAD)
            return;
        currentState_ = newState;

        switch (newState)
        {
            case State.READY: { Enter_ReadyState(); } break;
            case State.MOVE: { Enter_MoveState(); } break;
            case State.ATTACK: { Enter_AttackState(); } break;
            case State.DEAD: { Enter_DeadState(); } break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        /*if (Base)
        {
            if (!Base.CompareTag("Player"))
                Base = null;
        }
        if (!Base)
        {
            Base = GameObject.FindGameObjectWithTag("Player");
            NG = Base.GetComponent<NetGetMouse>();
        }*/
        switch (currentState_)
        {
            case State.READY: Update_ReadyState(); break;
            case State.MOVE: Update_MoveState(); break;
            case State.ATTACK: Update_AttackState(); break;
            case State.DEAD: Update_DeadState(); break;
        }
    }
    ///<summary>
    ///내가 사망했는지 체크하고, 사망한 경우는 코루틴으로 지연되는 처리를 모두 중지시키고, 사망 상태로만 전이시킨다.
    ///</summary>
    ///<param name = "myAttribute"></param>
    ///<returns></returns>
    bool ProcessDeadState(NetUnitAttribute myAttribute)
    {
        if (myAttribute.HP <= 0)
        {
            StopAllCoroutines();
            SetState(State.DEAD);
            return true;
        }
        return false;
    }
    /// <summary>
    /// 배치 전까지는 반투명 상태+마우스 이동
    /// </summary>
    void Enter_ReadyState()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        //gameObject.GetComponent<Collider>().enabled = false;
        //Mat = new Material[4];
        //col = new Color[4];
        //gameObject.AddComponent<NetGetMouse>();
        
        /*Renderer[] Rd = GetComponentsInChildren<Renderer>();

        for (int i = 0; i < Rd.Length; i++)
        {
            Mat[i] = Rd[i].material;
            col[i] = Mat[i].color;
        }*/
        //Cmd_AlphaChange(0.4f);
        //NetUnitAttribute myAttribute = gameObject.GetComponent<NetUnitAttribute>();
        //myAttribute.AlphaV = 0.4f;
    }
    void Update_ReadyState()
    {
        if (NG.IsPressedMouseButton == false)
        {
            MoveToMousePos();
        }
        if (NG.IsPressedMouseButton == true)
        {
            NetUnitAttribute myAttribute = gameObject.GetComponent<NetUnitAttribute>();
            myAttribute.HP = myAttribute.HpMAX;

            // myAttribute.AlphaV = 1f;

            Cmd_AlphaChange(1f);

            /*for (int i = 0; i < 4; i++)
            {
                col[i].a = a;
                Mat[i].color = col[i];
            }*/

            //AlphaChange(1f);
            
            SetState(State.MOVE);

            //gameObject.GetComponent<Collider>().enabled = true;
            //gameObject.GetComponent<GetMouse>().enabled = false;
        }
    }
    void Enter_MoveState()
    {
        NetUnitAttribute myAttribute = gameObject.GetComponent<NetUnitAttribute>();
        if (ProcessDeadState(myAttribute))
            return;
        anime.speed = 1.0f;
        anime.SetBool("Move", true);

        Update_MoveState();
    }
    void Update_MoveState()
    {
        NetUnitAttribute myAttribute = gameObject.GetComponent<NetUnitAttribute>();

        if (ProcessDeadState(myAttribute))
            return;
        //타겟이 계속 바뀌면 안됨으로,타겟이 유효한 경우는 유지되도록


        bool isFindTarget = false;
        if (Target != null)
        {
            NetUnitAttribute targetAttribute = Target.GetComponent<NetUnitAttribute>();
            if (targetAttribute.HP > 0)
                isFindTarget = true;
        }
        if (!isFindTarget)
        {
            Target = FindEnemy_MKII();
        }
        if (Target != null)
        {
            NetUnitAttribute targetAttribute = Target.GetComponent<NetUnitAttribute>();
            if (targetAttribute.HP <= 0)
                return;
            else if (nextAttackCooltime_ == 0.0f || nextAttackCooltime_ < Time.time)
            {
                nextAttackCooltime_ = Time.time + myAttribute.AttackTime;
                SetState(State.ATTACK);
            }
        }
        else
            transform.Translate(Vector3.forward.normalized * myAttribute.MoveSpeed * Time.deltaTime);
    }
    ///<summary>
    ///타겟에게 나의 능력치를 기반하여 HP를 감소시킨다.
    /// </summary>
    void Enter_AttackState()
    {
        NetUnitAttribute myAttribute = gameObject.GetComponent<NetUnitAttribute>();
        if (ProcessDeadState(myAttribute))
            return;
        anime.speed = 1 / (myAttribute.AttackTime * 2);
        anime.SetBool("Attack", true);
        NetUnitAttribute targetAttribute = Target.GetComponent<NetUnitAttribute>();
        targetAttribute.HP -= myAttribute.AttackPower;
        if (targetAttribute.HP <= 0)
        {
            Target = null;
            //Debug.Log("Target Changed");
            anime.SetBool("Attack", false);
            anime.speed = 0.2f;
            StartCoroutine("TransStateToMove", 0.5f);
        }
        else
            StartCoroutine("TransStateToMove", myAttribute.AttackTime);
    }
    void Update_AttackState()
    {
        NetUnitAttribute myAttribute = gameObject.GetComponent<NetUnitAttribute>();
        if (ProcessDeadState(myAttribute))
            return;
        if (Target != null)
        {
            NetUnitAttribute targetAttribute = Target.GetComponent<NetUnitAttribute>();
            if (targetAttribute.HP <= 0)
            {
                Target = null;
                //Debug.Log("Target Changed");
                anime.SetBool("Attack", false);
                anime.speed = 0.2f;
            }
        }
    }
    void Enter_DeadState()
    {
        anime.speed = 1f;
        anime.SetBool("Dead", true);
    }
    void Update_DeadState()
    {
        //Rpc_AlphaChange(a);
        //NetUnitAttribute myAttribute = gameObject.GetComponent<NetUnitAttribute>();
        //myAttribute.AlphaV = 1f;
        //myAttribute.AlphaV -= 0.05f;
        /*float a = 1f;
        Cmd_AlphaChange(a);
        a -= 0.05f;
        if (a <= 0f)
            NetObjectManager.Instance.Dead(gameObject);*/
    }
    [Command]
    void Cmd_AlphaChange(float a)
    {
        Renderer[] Rd = GetComponentsInChildren<Renderer>();
        Mat = new Material[4];
        col = new Color[4];

        for (int i = 0; i < Rd.Length; i++)
        {
            Mat[i] = Rd[i].material;
            col[i] = Mat[i].color;
            col[i].a = a;
            Mat[i].color = col[i];
        }
    }
    //[Command]
    void MoveToMousePos()
    {
        vecPos = NG.getTargetPos;
        vecPos.y = transform.position.y;
        if (!Mathf.Approximately(vecPos.x, 0) || !Mathf.Approximately(vecPos.z, 0))
        {
            vecPos.x = vecPos.x - (vecPos.x % 5);
            vecPos.z = vecPos.z - (vecPos.z % 5);
            if (Mathf.Approximately(vecPos.x % 10, 0))
                vecPos.x += 5;
            if (Mathf.Approximately(vecPos.z % 10, 0))
                vecPos.z += 5;
            transform.position = vecPos;
        }
    }
    GameObject FindEnemy_MKII()
    {
        return NetObjectManager.Instance.FindFirstEnemy_MKII(gameObject);
    }
    ///<summary>
    ///waitTime이 지난후에, Move상태로 전이한다.
    /// </summary>
    /// <param name = "waitTime"></param>
    /// <returns></returns>
    IEnumerator TransStateToMove(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        SetState(State.MOVE);
    }

    

}


