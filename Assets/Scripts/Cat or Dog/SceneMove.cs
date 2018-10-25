using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMove : MonoBehaviour {

    public GameObject Cat, Dog;
    Animator CatAnime,DogAnime;

    private void Awake()
    {
        CatAnime = Cat.GetComponent<Animator>();
        DogAnime = Dog.GetComponent<Animator>();
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 1000f))
        {
            if (hit.transform.name == "CatBackground")//고양이진영선택
            {
                CatAnime.SetBool("Dance", true);
                DogAnime.SetBool("Dance", false);
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("고양이눌림");
                }
            }
            else if (hit.transform.name == "DogBackground")//강아지진영선택
            {
                CatAnime.SetBool("Dance", false);
                DogAnime.SetBool("Dance", true);
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("강아지눌림");
                }
            }
            else
            {
                CatAnime.SetBool("Dance", false);
                DogAnime.SetBool("Dance", false);
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("둘다 안눌림");
                }
            }

        }
    }
}
