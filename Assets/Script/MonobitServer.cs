using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;
using System;
using UniRx;
using UniRx.Triggers;

using System.IO;

public class MonobitServer : MonobitEngine.MonoBehaviour
{
    [SerializeField]
    private byte maxPlayer = 10;
    [SerializeField]
    private int updateStreamRate = 60;
    [SerializeField]
    private int sendRate = 30;
	[SerializeField]
	private bool GUIDisplay = true;
	[SerializeField]
	private Color TestColor = Color.black;

    const string lobbyName = "TCALobby";
    const string roomName = "TCARoom";
    const string serverName = "TCAVRZemi";

	bool reconnect = false;
    System.IDisposable DisRemote;

    private void Start()
    {
        MonobitNetwork.updateStreamRate = updateStreamRate;
        MonobitNetwork.sendRate = sendRate;

        MonobitNetwork.autoJoinLobby = true;

        if (!MonobitNetwork.inRoom)
        {
            ConnectServer();
        }
    }

    private void OnDisconnectedFromServer()
    {
        Debug.Log("Disconnected.");
		if (reconnect) 
		{
            // Cannot connect immediately.....need to find out why...
            Observable.Timer(System.TimeSpan.FromSeconds(3)).Subscribe(_ =>
            {
                ConnectServer ();
            });
		}
    }

	private void ConnectServer()
	{
		Debug.Log("Connecting Server " + serverName + " ...");
		MonobitNetwork.ConnectServer(serverName);
	}

    private void OnJoinedLobby()
    {
        Debug.Log("Enter Lobby.");

        if (reconnect)
        {
            reconnect = false;
            DisRemote = this.UpdateAsObservable().Subscribe(_ =>
            {
                MonobitNetwork.GetRoomData().ToObservable()
                                            .Where(r => r.name == roomName && r.playerCount > 0)
                                            .Subscribe(r =>
                {
                    MonobitNetwork.JoinRoom(roomName);
                    DisRemote.Dispose();
                });
            });
        }
        else
        {
            MonobitEngine.RoomSettings settings = new MonobitEngine.RoomSettings();
            settings.maxPlayers = maxPlayer;
            settings.isVisible = true;
            settings.isOpen = true;
            MonobitEngine.LobbyInfo lobby = new MonobitEngine.LobbyInfo();
            lobby.Kind = LobbyKind.Default;
            lobby.Name = lobbyName;
            MonobitEngine.MonobitNetwork.JoinOrCreateRoom(roomName, settings, lobby);
        }
    }

    private void OnJoinedRoom()
    {
        Debug.Log("Enter Room.");
    }

	private void OnGUI()
	{
		if (GUIDisplay) 
		{
			GUI.color = TestColor;
			GUILayout.Label (MonobitNetwork.isConnect ? "Connected." : "Disconnected.");
			GUILayout.Label (MonobitNetwork.inRoom ? "In Room." : "Not In Room.");
			GUILayout.Label (MonobitNetwork.isHost ? "Host." : "Not Host.");	
		}
	}

	[MunRPC]
	void Reconnect()
	{
		if (MonobitNetwork.isConnect) 
		{
			reconnect = true;
			MonobitNetwork.DisconnectServer ();	
		} 
		else 
		{
			ConnectServer ();
		}
	}
}
