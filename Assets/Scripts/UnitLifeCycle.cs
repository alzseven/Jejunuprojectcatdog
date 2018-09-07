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
    public Now now;
    private Vector3 vecPos = Vector3.zero;
    private Vector3 VFront = Vector3.zero;
    Material[] Mat;
    Color[] col;
    Animator anime;

    void Start()
    {
        anime = GetComponent<Animator>();
        gameObject.GetComponent<Collider>().enabled = false;
        Mat = new Material[3];
        col = new Color[3];
        GM = gameObject.AddComponent<GetMouse>();
        now = Now.Ready;
        Renderer[] Rd = GetComponentsInChildren<Renderer>();
        for (var i=0 ; i < Rd.Length; i++) {
            Mat[i] = Rd[i].material;
            col[i] = Mat[i].color;    
        }
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
                Debug.Log(tempPosition);
                transform.position = tempPosition;
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
            anime.SetBool("IsMoving", true);
            transform.Translate(Vector3.forward.normalized * 5f * Time.deltaTime,Space.World);
        }
    }
    void AlphaChange(float a)
    {
        for (int i = 0; i < 3; i++)
        {
            col[i].a = a;
            Mat[i].color = col[i];
        }
    }
}
