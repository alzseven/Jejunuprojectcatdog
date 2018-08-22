using UnityEngine;

public class NewMovement : MonoBehaviour
{
    //private Transform tr = null;
    private Vector3 vecPos = Vector3.zero;
    private bool isObjectMoving = false;

    private void Awake()
    {
        //tr = transform;
    }
    private void Update()
    {
        if (isObjectMoving)
        {
            transform.position = vecPos;
        }

    }
    public void ToPosition(Vector3 toPos)
    {
        vecPos = toPos;
        vecPos.y = transform.position.y;
        isObjectMoving = true;
    }
}
