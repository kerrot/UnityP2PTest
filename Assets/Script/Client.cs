using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System;
using UniRx;
using UniRx.Triggers;

public class Client : MonoBehaviour {
    [SerializeField]
    private string IP;
    [SerializeField]
    private int port;
    [SerializeField]
    private GameObject obj;

    Thread thread;

    TcpClient tcpClient = new TcpClient();
    NetworkStream stream;

    Vector3 position;

    private void Start()
    {
        thread = new Thread(ThreadWork);

        thread.Start();
    }

    private void Update()
    {
        position = obj.transform.position;

        if (stream != null)
        {
            byte[] data = System.Text.Encoding.ASCII.GetBytes(string.Format("{0},{1},{2}", position.x, position.y, position.z));
            stream.Write(data, 0, data.Length);

            Debug.Log("Send: " + data);
        }
        
    }

    void ThreadWork()
    {
        tcpClient.Connect(IP, port);
        Debug.Log("Client connected");


        stream = tcpClient.GetStream();

    }

    private void OnDestroy()
    {
        if (stream != null)
        {
            stream.Close();
        }
        
        tcpClient.Close();
    }
}
