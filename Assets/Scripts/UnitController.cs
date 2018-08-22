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

    private MouseCtrlManager MCtrl = null;
    private Vector3 vecPos = Vector3.zero;
    private bool isObjectMoving = false;

    // Use this for initialization
    void Start () {
        MCtrl = gameObject.AddComponent<MouseCtrlManager>();
    }

    // Update is called once per frame
    void Update () {

        if (isObjectMoving)
        {
            transform.position = vecPos;
        }
        if (MCtrl.IsPressedMouseButton == false)
        {
            vecPos = MCtrl.V3targPos;
            vecPos.y = transform.position.y;
            isObjectMoving = true;
        }
    }
}
