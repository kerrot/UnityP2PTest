using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using System.Runtime.InteropServices;

public class DataLightColor : DataBase
{
    float[] color = new float[4];

    public DataLightColor()
    {
        dataLength = Marshal.SizeOf(typeof(float)) * 4;
    }

    public override void Apply(GameObject obj, List<byte> data)
    {
        Light l = obj.GetComponent<Light>();
        if (l)
        {
            int unit = Marshal.SizeOf(typeof(float));
            byte[] tmpData = data.ToArray();

            color[0] = BitConverter.ToSingle(tmpData, 0);
            color[1] = BitConverter.ToSingle(tmpData, unit);
            color[2] = BitConverter.ToSingle(tmpData, unit * 2);
            color[3] = BitConverter.ToSingle(tmpData, unit * 3);
            l.color = new Color(color[0], color[1], color[2], color[3]);
        }
    }

    public override List<byte> UpdateData(GameObject obj)
    {
        Light l = obj.GetComponent<Light>();
        if (l)
        {
            color[0] = l.color.r;
            color[1] = l.color.g;
            color[2] = l.color.b;
            color[3] = l.color.a;

            data.Clear();
            data.Add((byte)DataType.DataLightColor);
            data.AddRange(color.ToByteArray());
        }

        return data;
    }
}
