using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace MultiPlayer
{
    

}
public class GameTimer : NetworkBehaviour
{
    //[SyncVar] public float gameTime; //The length of a game, in seconds.
    [SyncVar] public float timer; //How long the game has been running. -1=waiting for players, -2=game is done
                                  //[SyncVar] public float OneSec;
                                  //[SyncVar] public int minPlayers; //Number of players required for the game to start
    [SyncVar] public bool masterTimer = false; //Is this the master timer?
                                               //public ServerTimer timerObj;

    public Text sourceText;
    //[SyncVar] public float source = 0f;
    public float s_perTime = 1;
    [SyncVar] public float OneSecCount = 0;
    public Text ServerTime;

    GameTimer serverTimer;
    NewSpawner NS;
    void Start()
    {
        NS = gameObject.GetComponent<NewSpawner>();
        sourceText = GameObject.FindGameObjectWithTag("Resource").GetComponent<Text>();
        ServerTime = GameObject.FindGameObjectWithTag("Timer").GetComponent<Text>();
        if (isServer)
        { // For the host to do: use the timer and control the time.
            if (isLocalPlayer)
            {
                serverTimer = this;
                masterTimer = true;
                NS.source += 10f;
            }
        }
        else if (isLocalPlayer)
        { //For all the boring old clients to do: get the host's timer.
            GameTimer[] timers = FindObjectsOfType<GameTimer>();
            for (int i = 0; i < timers.Length; i++)
            {
                if (timers[i].masterTimer)
                {
                    serverTimer = timers[i];
                }
            }
            NS.source += 10f;
        }

    }
    /*[Command]
    void CmdSourcePlus()
    {
        RpcSourcePlus();
    }
    [ClientRpc]
    void RpcSourcePlus()
    {

    }*/
    void Update()
    {
        if (masterTimer)
        { //Only the MASTER timer controls the time
          /*if (timer >= gameTime)
          {
              timer = -2;
          }
          else if (timer == -1)
          {
              if (NetworkServer.connections.Count >= minPlayers)
              {
                  timer = 0;
              }
          }
          else if (timer == -2)
          {
              //Game done.
          }
          else
          {*/
            timer += Time.deltaTime;
            //OneSec += Time.deltaTime;
            //}
        }

        if (isLocalPlayer)
        { //EVERYBODY updates their own time accordingly.
            if (serverTimer)
            {
                //gameTime = serverTimer.gameTime;
                timer = serverTimer.timer;
                //OneSec = serverTimer.OneSec;
                OneSecCount += Time.deltaTime;
                if (OneSecCount >= 1)
                {
                    NS.source += (int)OneSecCount * s_perTime;
                    OneSecCount -= OneSecCount;
                }
                sourceText.text = string.Format("{0:N0}", NS.source) + "사료";
                int minute = (int)timer / 60;
                if (timer < 60)
                {
                    minute = 0;
                }
                int second = (int)timer % 60;
                ServerTime.text = minute + ":" + second;
                //minPlayers = serverTimer.minPlayers;
            }
            else
            { //Maybe we don't have it yet?
                GameTimer[] timers = FindObjectsOfType<GameTimer>();
                for (int i = 0; i < timers.Length; i++)
                {
                    if (timers[i].masterTimer)
                    {
                        serverTimer = timers[i];
                    }
                }
            }
        }
    }
}
