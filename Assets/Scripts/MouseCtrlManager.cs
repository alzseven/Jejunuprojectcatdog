using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCtrlManager : MonoBehaviour
{
    private UnitController UC;
    private bool isPressedMouseButton = false;
    public bool IsPressedMouseButton { get { return isPressedMouseButton; } }
    private Vector3 V3TargPos = Vector3.zero;
    public Vector3 V3targPos { get { return V3TargPos; } }

    public Renderer ObstacleRenderer;

    // Use this for initialization
    void Start()
    {
        //UC = gameObject.AddComponent<UnitController>();
    }

    // Update is called once per frame
    void Update()
    {
        var UC = gameObject.GetComponent<UnitController>();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 1000f))
        {
            V3TargPos = hit.point;
            if (hit.collider.tag == "unit")
            {
                ObstacleRenderer = hit.collider.GetComponentInChildren<Renderer>();
                if (ObstacleRenderer != null && isPressedMouseButton != true && UC.state==State.Ready)

                {
                    Material Mat = ObstacleRenderer.material;

                    Color matColor = Mat.color;

                    matColor.a = 0.4f;

                    Mat.color = matColor;

                    Debug.Log(hit.transform.name);
                }
                

            }
            if (Input.GetMouseButtonDown(0))
            {

                isPressedMouseButton = true;

                /*Material Mat = ObstacleRenderer.material;

                Color matColor = Mat.color;

                matColor.a = 1f;

                Mat.color = matColor;*/
            }
        }
    }
}
