using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;

public class MonobitComponent : MonobitEngine.MonoBehaviour
{
    public struct MonobitObject
    {
        public string name;
        public Vector3 position;
        public Quaternion rotation;
    }

    private void Awake()
    {
        MonobitSever server = GameObject.FindObjectOfType<MonobitSever>();
        if (server)
        {
            server.AddObject(new MonobitObject() { name = gameObject.name, position = transform.position, rotation = transform.rotation });
        }
        DestroyObject(gameObject);
    }
}
