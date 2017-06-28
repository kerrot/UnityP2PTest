using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;
using System;
using UniRx;
using UniRx.Triggers;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class MonobitRPC : MonobitEngine.MonoBehaviour
{
    [SerializeField]
    private float updatePeriod = 1;

    [Serializable]
    public struct DataPackage
    {
        public int ID;
        public List<DataBase.DataDetail> Data;
    }

    int count = 0;
    string msg;
    void OnGUI()
    {
        GUI.color = Color.white;
        GUILayout.Label(msg);
    }

    List<DataPackage> packages = new List<DataPackage>();

    private void Start()
    {
        Observable.Interval(TimeSpan.FromSeconds(updatePeriod)).Subscribe(_ => UniRxLateUpdate());
    }

    private void UniRxLateUpdate()
    {
        if (!enabled || !MonobitNetwork.isHost || !MonobitNetwork.inRoom)
        {
            return;
        }

        packages.Clear();

        DateHolder[] holders = GameObject.FindObjectsOfType<DateHolder>();
        holders.ToObservable().Subscribe(h =>
        {
            MonobitView view = h.gameObject.GetComponent<MonobitView>();
            if (view)
            {
                DataPackage pack = new DataPackage();
                pack.ID = view.viewID;
                pack.Data = h.GetData();

                packages.Add(pack);
            }
        });

        if (packages.Count > 0)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter myBinaryFormatter = new BinaryFormatter();
            myBinaryFormatter.Serialize(stream, packages);

            monobitView.RPC("RecvUpdate", MonobitTargets.Others, stream.ToArray());
        }
    }

    [MunRPC]
    void RecvUpdate(byte[] buffer)
    {
        msg = count++.ToString() + " ";

        MemoryStream stream = new MemoryStream(buffer);
        BinaryFormatter myBinaryFormatter = new BinaryFormatter();
        List<DataPackage> tmpData = (List<DataPackage>)myBinaryFormatter.Deserialize(stream);

        DateHolder[] holders = GameObject.FindObjectsOfType<DateHolder>();
        holders.ToObservable().Subscribe(h => h.Apply(tmpData));
    }
}
