using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;
using System;
using System.Linq;
using UniRx;
using UniRx.Triggers;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;

public class MonobitRPC : MonobitEngine.MonoBehaviour
{
    [SerializeField]
    private float updatePeriod = 1;

    List<byte> packages = new List<byte>();

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
            List<byte> tmp = h.GetData();
            packages.AddRange((BitConverter.GetBytes(tmp.Count)));
            packages.AddRange(tmp);
        });

        if (packages.Count > 0)
        {
            monobitView.RPC("RecvUpdate", MonobitTargets.All, packages.ToArray());
        }
    }


    int debug = 0;
    string debugMsg;
    void OnGUI()
    {
        GUI.color = Color.white;
        GUILayout.Label(debugMsg);
    }

    [MunRPC]
    void RecvUpdate(byte[] buffer)
    {
        debugMsg = debug++.ToString() + " " + buffer.Length.ToString();

        List<byte> total = buffer.ToList();

        if (total.Count > 0)
        {
            int count = 0;
            int intSize = Marshal.SizeOf(typeof(int)); ;

            DateHolder[] holders = GameObject.FindObjectsOfType<DateHolder>();

            do
            {
                int length = BitConverter.ToInt32(buffer, count);
                if (total.Count >= length + count + intSize)
                {
                    List<byte> tmp = total.GetRange(count + intSize, length);
                    int id = BitConverter.ToInt32(tmp.ToArray(), 0);
                    DateHolder holder = holders.SingleOrDefault(h => h.ID == id);
                    if (holder)
                    {
                        holder.Apply(tmp.GetRange(intSize, tmp.Count - intSize));
                    }
                }

                count += intSize + length;
            } while (total.Count > count);
        }
    }
}
