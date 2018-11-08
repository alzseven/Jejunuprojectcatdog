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
        NS.DeckList0 = lobby.decklist[0];
        NS.DeckList1 = lobby.decklist[1];
        NS.DeckList2 = lobby.decklist[2];
        NS.DeckList3 = lobby.decklist[3];
        NS.DeckList4 = lobby.decklist[4];
        NS.cat = lobby.cat;
    }
}

