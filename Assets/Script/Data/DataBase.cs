using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using System.IO;

public abstract class DataBase
{
    protected int dataLength;
    public int DataLength { get { return dataLength; } }

    protected List<byte> data = new List<byte>();

    public abstract void Apply(GameObject obj, List<byte> data);
    public abstract List<byte> UpdateData(GameObject obj);

    
    //protected Type Deserialize<Type>(byte[] data)
    //{
    //    MemoryStream stream = new MemoryStream(data);
    //    BinaryFormatter myBinaryFormatter = new BinaryFormatter();
    //    return (Type)myBinaryFormatter.Deserialize(stream);
    //}

    //protected byte[] Serialize<Type>(object data)
    //{
    //    MemoryStream stream = new MemoryStream();
    //    BinaryFormatter myBinaryFormatter = new BinaryFormatter();
    //    myBinaryFormatter.Serialize(stream, data);

    //    return stream.ToArray();
    //}
}
