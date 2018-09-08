using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour {
    Animator anime;

    private void Start()
    {
        anime = GetComponent<Animator>();
    }
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 1000f))
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.name == "CatBackground")
            {
                anime.SetBool("CatOnMouse", true);
                anime.SetBool("DogOnMouse", false);
            }
            if (hit.transform.name == "DogBackground")
            {
                anime.SetBool("DogOnMouse", true);
                anime.SetBool("CatOnMouse", false);
            }
            if (hit.transform.name == "Plane")
            {
                anime.SetBool("CatOnMouse", false);
                anime.SetBool("DogOnMouse", false);
            }
        }
    }
}
