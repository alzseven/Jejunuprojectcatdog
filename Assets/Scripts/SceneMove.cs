using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMove : MonoBehaviour {
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 1000f))
            if (hit.transform.name == "CatBackground")//고양이진영선택
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("고양이눌림");
            }
        }
        if (hit.transform.name == "DogBackground")//강아지진영선택
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("강아지눌림");
            }
        }
    }
}
