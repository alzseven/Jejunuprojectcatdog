using UnityEngine;

public class NewInputPlayer : MonoBehaviour
{
    private bool isPressedMouseButton = false;
    private Vector3 getTargetPos = Vector3.zero;
    public bool IsPressedMouseButton { get { return isPressedMouseButton; } }
    public Vector3 GetTargetPos { get { return getTargetPos; } }


    


    void Update()
    {
        isPressedMouseButton = false;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 1000f))
        {
            getTargetPos = hit.point;
        }
        /*if (Input.GetMouseButtonDown(0))
        {
            isPressedMouseButton = true;
            

        }*/
    }
}
