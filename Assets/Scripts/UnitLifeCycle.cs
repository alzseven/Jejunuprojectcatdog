using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Now
{
    Ready,
    Move,
    Attack,
    Die
}
public class UnitLifeCycle : MonoBehaviour
{
    private GetMouse GM = null;
    private Vector3 vecPos = Vector3.zero;
    Material[] Mat;
    Color[] col;
    public int RDL = 0;
    
    Animator anime;

    GameObject Target;
    UnitLifeCycle GivingDamage;

    public Now now;
    public float Speed = 0f;
    public int maxHealth = 0;
    public float Range = 0f;
    public int AttackPower = 0;
    public float AttackSpeed = 0f;
    public bool IsHit = false;

    public int Health = 0;

    void Start()
    {
        Health = maxHealth;
        anime = GetComponent<Animator>();
        gameObject.GetComponent<Collider>().enabled = false;
        Mat = new Material[3];
        col = new Color[3];
        GM = gameObject.AddComponent<GetMouse>();
        now = Now.Ready;
        Renderer[] Rd = GetComponentsInChildren<Renderer>();
        for (int i = 0; i < Rd.Length; i++)
        {
            Mat[i] = Rd[i].material;
            col[i] = Mat[i].color;
        }
        RDL = Rd.Length;
        AlphaChange(0.4f);
    }
    void Update()
    {
        if (now == Now.Ready)
        {
            if (GM.IsPressedMouseButton == false)
            {
                vecPos = GM.GetTargetPos;
                vecPos.y = transform.position.y;
                var tempPosition = vecPos;
                Debug.Log(tempPosition);
                if (!Mathf.Approximately(vecPos.x, 0) || !Mathf.Approximately(vecPos.z, 0))
                {
                    tempPosition.x = tempPosition.x - (tempPosition.x % 5);// -5f;
                    tempPosition.z = tempPosition.z - (tempPosition.z % 5);// -5f;
                    if (Mathf.Approximately(tempPosition.x % 10, 0))
                    {
                        tempPosition.x += 5;
                    }
                    if (Mathf.Approximately(tempPosition.z % 10, 0))
                    {
                        tempPosition.z += 5;
                    }
                    transform.position = tempPosition;
                }

            }
            if (GM.IsPressedMouseButton == true)
            {
                now = Now.Move;
                AlphaChange(1f);
                gameObject.GetComponent<Collider>().enabled = true;
            }
        }
        if (now == Now.Move)
        {
            if (Health <= 0)
            {
                Destroy(this.gameObject);
            }
            Ray ray = new Ray(this.gameObject.transform.position, Vector3.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Range))//&& hit.collider.tag != "ground")
            {
                Target = hit.collider.gameObject;
                GivingDamage = Target.GetComponent<UnitLifeCycle>();
                now = Now.Attack;
                Debug.Log(hit.transform.name);
            }
            anime.SetBool("Move", true);
            transform.Translate(Vector3.forward.normalized * Speed * Time.deltaTime);//, Space.World);

        }
        if (now == Now.Attack)
        {
            if (Health <= 0)
            {
                Destroy(this.gameObject);
            }
            if (IsHit == false)
            {
                IsHit = true;
                anime.speed = 0.5f;
                anime.SetBool("Attack", true);

                StartCoroutine("Attack", AttackSpeed);
            }
            Ray ray = new Ray(this.gameObject.transform.position, Vector3.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Range))//&& hit.collider.tag != "ground")
            {
                Target = hit.collider.gameObject;
                Debug.Log(hit.transform.name);
            }
            if (!Physics.Raycast(ray.origin, ray.direction, out hit, Range))//&& hit.collider.tag != "ground")
            {
                anime.speed = 1f;
                anime.SetBool("IsAttacking", false);
                now = Now.Move;
            }
        }
    }

    void AlphaChange(float a)
    {
        for (int i = 0; i < RDL; i++)
        {
            col[i].a = a;
            Mat[i].color = col[i];
        }
    }
    IEnumerator Attack(float Delay)
    {
        yield return new WaitForSeconds(Delay);
        GivingDamage.TakeDamage(AttackPower);
        IsHit = false;
    }
    void TakeDamage(int amount)
    {
        Health -= amount;
    }
}
