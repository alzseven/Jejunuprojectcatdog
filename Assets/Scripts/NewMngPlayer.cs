using UnityEngine;

[RequireComponent(typeof(NewMovement))]
public class NewMngPlayer : MonoBehaviour
{
    private NewInputPlayer inputPlayer = null;
    private NewMovement movementPlayer = null;
    void Start()
    {
        inputPlayer = this.gameObject.AddComponent<NewInputPlayer>();
        if (inputPlayer == null) { inputPlayer = this.GetComponent<NewInputPlayer>(); }
        movementPlayer = this.GetComponent<NewMovement>();
    }

    void Update()
    {
        if (inputPlayer.IsPressedMouseButton==false)
        {
            movementPlayer.ToPosition(inputPlayer.GetTargetPos);
        }
    }

}

