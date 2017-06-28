using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public abstract class DataBase
{
    [Serializable]
    public struct DataDetail
    {
        public DataType Type;
        public byte[] Data;
    }

    public abstract void Apply(GameObject obj, DataDetail data);
    public abstract DataDetail UpdateData(GameObject obj);

    
    protected Type Deserialize<Type>(byte[] data)
    {
        MemoryStream stream = new MemoryStream(data);
        BinaryFormatter myBinaryFormatter = new BinaryFormatter();
        return (Type)myBinaryFormatter.Deserialize(stream);
    }

    protected byte[] Serialize<Type>(object data)
    {
        MemoryStream stream = new MemoryStream();
        BinaryFormatter myBinaryFormatter = new BinaryFormatter();
        myBinaryFormatter.Serialize(stream, data);

        return stream.ToArray();
    }
}
