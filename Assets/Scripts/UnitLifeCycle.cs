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
    public Material M1, M2, M3;
    private Vector3 VFront = Vector3.zero;

    void Start()
    {
        GM = gameObject.AddComponent<GetMouse>();
        //State를 Ready로 설정
        now = Now.Ready;
        Color M1col = M1.color;
        Color M2col = M2.color;
        Color M3col = M3.color;
        M1col.a = 0.4f;
        M2col.a = 0.4f;
        M3col.a = 0.4f;
        M1.color = M1col;
        M2.color = M2col;
        M3.color = M3col;
        //Material의 클론을 만들어서 오브잭트의 클론으로 설정
    }
    void Update()
    {
        if (now == Now.Ready)
        {
            //마우스 이동을 받아와야 함	-다른 스크립트
            if (GM.IsPressedMouseButton == false)
            {
                PosSet(GM.GetTargetPos);
            }
            //마우스 좌표 보정, 마우스 왼쪽버튼이 클릭되기 전까지
            if (GM.IsPressedMouseButton == true)  //마우스 왼쪽 버튼이 눌렸을때
            {
                now = Now.Move;
                AlphaReset();
            }
            //유닛이 불투명해짐+상태 변경
        }
        if (now == Now.Move)
        {
            transform.Translate(Vector3.forward.normalized * 10f * Time.deltaTime);//글로벌 좌표 기준 유닛의 전방으로 이동해야함
        }
    }
    void PosSet(Vector3 vecPos)
    {
        vecPos.y = transform.position.y;
        var tempPosition = vecPos;
        tempPosition.x = tempPosition.x - (tempPosition.x % 5);// + 5f;
        tempPosition.z = tempPosition.z - (tempPosition.z % 5);// + 5f;
        transform.position = tempPosition;
    }
    void AlphaReset()
    {
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
}
