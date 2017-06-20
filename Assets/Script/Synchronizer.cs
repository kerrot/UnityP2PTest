using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Net;
using System.Net.Sockets;

public class Synchronizer : MonoBehaviour {
    [SerializeField]
    private string IP;
    [SerializeField]
    private int port;

    Thread thread;

    private void Start()
    {
        thread = new Thread(ThreadWork);

        thread.Start();
    }

    private void Update()
    {

    }

    void ThreadWork()
    {
        IPAddress ipAdd = IPAddress.Parse(IP);
        TcpListener listener = new TcpListener(ipAdd, port);
        listener.Start();
        Debug.Log("TCP listening...");

        TcpClient client = listener.AcceptTcpClient();
        Debug.Log("TCP Connected.");
    }
}
