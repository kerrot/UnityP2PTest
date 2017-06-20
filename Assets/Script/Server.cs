using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using UniRx;
using UniRx.Triggers;

public class Server : MonoBehaviour {
    [SerializeField]
    private string IP;
    [SerializeField]
    private int port;
    [SerializeField]
    private GameObject obj;

    Thread thread;

    TcpClient client;
    NetworkStream stream;

    Vector3 position;

    private void Awake()
    {
        position = obj.transform.position;
    }

    private void Start()
    {
        thread = new Thread(ThreadWork);

        thread.Start();
    }

    private void Update()
    {
        obj.transform.position = position;
    }

    void ThreadWork()
    {
        IPAddress ipAdd = IPAddress.Parse(IP);
        TcpListener listener = new TcpListener(ipAdd, port);
        listener.Start();
        Debug.Log("Server listening...");

        client = listener.AcceptTcpClient();
        Debug.Log("Server Connected.");

        stream = client.GetStream();

        byte[] data = new byte[256];

        while (true)
        {
            int bytes = stream.Read(data, 0, data.Length);

            string s = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            string[] p = s.Split(',');
            position.x = float.Parse(p[0]);
            position.y = float.Parse(p[1]);
            position.z = float.Parse(p[2]);
            Debug.Log("Recieve: " + position);
        }
    }

    private void OnDestroy()
    {
        if (stream != null)
        {
            stream.Close();
        }

        if (client != null)
        {
            client.Close();
        }
    }
}
