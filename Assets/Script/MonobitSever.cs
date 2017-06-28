using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;
using System;
using UniRx;
using UniRx.Triggers;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class MonobitSever : MonobitEngine.MonoBehaviour
{
    [SerializeField]
    private byte maxPlayer = 2;
    [SerializeField]
    private int updateStreamRate = 60;
    [SerializeField]
    private int sendRate = 30;

    const string lobbyName = "TCALobby";
    const string roomName = "TCARoom";
    const string serverName = "TCAVRZemi";

    List<MonobitComponent.MonobitObject> objs = new List<MonobitComponent.MonobitObject>();

    private void Start()
    {
        MonobitNetwork.updateStreamRate = updateStreamRate;
        MonobitNetwork.sendRate = sendRate;

        MonobitNetwork.autoJoinLobby = true;
        MonobitNetwork.ConnectServer(serverName);
    }

    private void OnDisconnectedFromServer()
    {
        Debug.Log("Disconnected.");
    }

    private void OnJoinedLobby()
    {
        Debug.Log("Enter Lobby.");

        MonobitEngine.RoomSettings settings = new MonobitEngine.RoomSettings();
        settings.maxPlayers = maxPlayer;
        settings.isVisible = true;
        settings.isOpen = true;
        MonobitEngine.LobbyInfo lobby = new MonobitEngine.LobbyInfo();
        lobby.Kind = LobbyKind.Default;
        lobby.Name = lobbyName;
        MonobitEngine.MonobitNetwork.JoinOrCreateRoom(roomName, settings, lobby);
    }

    private void OnJoinedRoom()
    {
        Debug.Log("Enter Room.");

        if (MonobitNetwork.isHost)
        {
            objs.ForEach(o =>
            {
                UnityEngine.Object m = Resources.Load(o.name);
                GameObject tmp = MonobitNetwork.Instantiate(o.name, o.position, o.rotation, 0);
            });
        }
    }

    public void AddObject(MonobitComponent.MonobitObject obj)
    {
        objs.Add(obj);
    }
}
