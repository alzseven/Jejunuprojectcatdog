using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour {

    public enum ANIMAL
    {
        INITALIZE,
        CAT,
        DOG
    }
    public string PlayerName;
    public ANIMAL animal;
    public string[] cards;

    public void GetPlayerName(InputField playername)
    {
        PlayerName = playername.text;
        Debug.Log(PlayerName);
    }
	// Use this for initialization
	public void PressStartButton () {
		
	}
    void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
