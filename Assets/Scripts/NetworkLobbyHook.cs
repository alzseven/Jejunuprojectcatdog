using UnityEngine;
using Prototype.NetworkLobby;
using System.Collections;
using UnityEngine.Networking;

public class NetworkLobbyHook : LobbyHook
{
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
        NewSpawner NS = gamePlayer.GetComponent<NewSpawner>();
        /*NetworkSpaceship spaceship = gamePlayer.GetComponent<NetworkSpaceship>();

        spaceship.name = lobby.name;
        spaceship.color = lobby.playerColor;
        spaceship.score = 0;
        spaceship.lifeCount = 3;*/

        NS.TeamColor = lobby.playerColor;
        NS.HpBarName = lobby.playerName;
        for (int i = 0; i < 5; i++)
        {
            NS.DeckList[i] = lobby.decklist[i];
        }
        NS.cat = lobby.cat;
    }
}

