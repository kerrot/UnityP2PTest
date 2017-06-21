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
    private byte maxPlayer = 10;
    [SerializeField]
    private int updateStreamRate = 60;
    [SerializeField]
    private int sendRate = 60;

    const string lobbyName = "TCALobby";
    const string roomName = "TCARoom";
    const string serverName = "TCAVRZemi";

    Dictionary<int, DateHolder> holders = new Dictionary<int, DateHolder>();

    [Serializable]
    public class DataPackage
    {
        public int ID;
        public List<DataBase> Data;
    }

    List<DataPackage> package = new List<DataPackage>();

    private void Awake()
    {
        MonobitView view = GetComponent<MonobitView>();
        if (view)
        {
            view.ObservedComponents.ForEach(c =>
            {
                DateHolder holder = c.gameObject.GetComponent<DateHolder>();
                if (holder)
                {
                    holder.ID = view.ObservedComponents.IndexOf(c);
                    holders.Add(holder.ID, holder);
                }
            });
        }
    }

    private void Start()
    {
        MonobitNetwork.updateStreamRate = updateStreamRate;
        MonobitNetwork.sendRate = sendRate;

        MonobitNetwork.autoJoinLobby = true;
        MonobitNetwork.ConnectServer(serverName);

        Observable.Interval(TimeSpan.FromSeconds(sendRate)).Subscribe(_ => UniRxLateUpdate());

        //this.LateUpdateAsObservable().Subscribe(_ => UniRxLateUpdate());
    }

    private void UniRxLateUpdate()
    {
        if (!enabled || !MonobitNetwork.isHost || !MonobitNetwork.inRoom)
        {
            return;
        }

        package.Clear();

        holders.ToObservable().Subscribe(h =>
        {
            DataPackage tmp = new DataPackage() { ID = h.Key, Data = h.Value.GetData() };
            package.Add(tmp);
        });

        if (package.Count > 0)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter myBinaryFormatter = new BinaryFormatter();
            myBinaryFormatter.Serialize(stream, package);

            monobitView.RPC("RecvUpdate", MonobitTargets.Others, stream.ToArray());
        }
    }

    [MunRPC]
    void RecvUpdate(byte[] buffer)
    {
        MemoryStream stream = new MemoryStream(buffer);
        BinaryFormatter myBinaryFormatter = new BinaryFormatter();
        List<DataPackage> package = (List<DataPackage>)myBinaryFormatter.Deserialize(stream);

        package.ForEach(p =>
        {
            if (holders.ContainsKey(p.ID))
            {
                p.Data.ForEach(d => d.Apply(holders[p.ID].gameObject));
            }
        });
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
        if (MonobitEngine.MonobitNetwork.JoinOrCreateRoom(roomName, settings, lobby))
        {
            Debug.Log("Enter Room.");
        }
        else
        {
            Debug.Log("Enter Room failed.");
        }
    }
}
