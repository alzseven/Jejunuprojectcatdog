using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMouse : MonoBehaviour {

    private bool isPressedMouseButton = false;
    private Vector3 getTargetPos = Vector3.zero;
    public bool IsPressedMouseButton { get { return isPressedMouseButton; } }
    public Vector3 GetTargetPos { get { return getTargetPos; } }

    void Update () {
        isPressedMouseButton = false;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 1000f)&& hit.collider.tag == "ground")
        {
           getTargetPos = hit.point;
           //Debug.Log(hit.transform.name);
        }
        if (Input.GetMouseButtonDown(0))
        {
            isPressedMouseButton = true;
        }
    }
}
