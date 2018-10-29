using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Prototype.NetworkLobby
{
    //Main menu, mainly only a bunch of callback called by the UI (setup throught the Inspector)
    public class AnimineControl : MonoBehaviour
    {

        public GameObject Cat, Dog;
        static public Animator CatAnime, DogAnime;

        private void OnEnable()
        {
            CatAnime = Cat.GetComponent<Animator>();
            DogAnime = Dog.GetComponent<Animator>();
        }



    }
}
