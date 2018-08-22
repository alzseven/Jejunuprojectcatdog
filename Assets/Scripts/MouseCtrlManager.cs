using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCtrlManager : MonoBehaviour
{
    private bool isPressedMouseButton = false;
    public bool IsPressedMouseButton { get { return isPressedMouseButton; } }
    private Vector3 V3TargPos = Vector3.zero;
    public Vector3 V3targPos { get { return V3TargPos; } }

    Renderer ObstacleRenderer;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 1000f))
        {
            V3TargPos = hit.point;
            ObstacleRenderer = hit.collider.GetComponentInChildren<Renderer>();

            if (ObstacleRenderer != null && IsPressedMouseButton != true)

            {
                Material Mat = ObstacleRenderer.material;

                Color matColor = Mat.color;

                matColor.a = 0.5f;

                Mat.color = matColor;

                //Debug.Log("AlphaChanged");
            }
        }
        if (Input.GetMouseButtonDown(0))
        {

            isPressedMouseButton = true;

            Material Mat = ObstacleRenderer.material;

            Color matColor = Mat.color;

            matColor.a = 1f;

            Mat.color = matColor;
        }
    }
}
