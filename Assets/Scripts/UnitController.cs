using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State{
    Ready,
    Move,
    Attack,
    Die
 }
public class UnitController : MonoBehaviour {

    private MouseCtrlManager MCtrl= null;
    public State state;
    private Vector3 vecPos = Vector3.zero;
    private bool isObjectMoving = false;
    public Material M1, M2, M3;
    private Vector3 VFront = Vector3.zero;

    // Use this for initialization
    void Start () {

        MCtrl = gameObject.AddComponent<MouseCtrlManager>();
        state = State.Ready;
        Color M1col = M1.color;
        Color M2col = M2.color;
        Color M3col = M3.color;
        M1col.a = 0.4f;
        M2col.a = 0.4f;
        M3col.a = 0.4f;
        M1.color = M1col;
        M2.color = M2col;
        M3.color = M3col;
    }

    // Update is called once per frame
    void Update () {
        
        if (isObjectMoving == true)
        {
            var tempPosition = vecPos;
            tempPosition.x = tempPosition.x - (tempPosition.x % 5);// + 5f;
            tempPosition.z = tempPosition.z - (tempPosition.z % 5);// + 5f;
            transform.position = tempPosition;
            //Debug.Log(tempPosition);
        }
        if (MCtrl.IsPressedMouseButton == false)
        {
            vecPos = MCtrl.V3targPos;
            vecPos.y = transform.position.y;
            isObjectMoving = true;
        }
        if (MCtrl.IsPressedMouseButton == true)
        {
            state = State.Move;
            isObjectMoving = false;

            Color M1col = M1.color;
            Color M2col = M2.color;
            Color M3col = M3.color;
            M1col.a = 1f;
            M2col.a = 1f;
            M3col.a = 1f;
            M1.color = M1col;
            M2.color = M2col;
            M3.color = M3col;
        }
        if (state == State.Move)
        {
            VFront = new Vector3(0, 0, 1);
            transform.Translate(Vector3.forward.normalized * 10f * Time.deltaTime);
        }
    }
}
