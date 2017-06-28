using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using System.Runtime.InteropServices;

public class DataLightColor : DataBase
{
    float[] color = new float[4];

    public override void Apply(GameObject obj, DataDetail data)
    {
        Light l = obj.GetComponent<Light>();
        if (l)
        {
            int unit = Marshal.SizeOf(typeof(float));

            color[0] = BitConverter.ToSingle(data.Data, 0);
            color[1] = BitConverter.ToSingle(data.Data, unit);
            color[2] = BitConverter.ToSingle(data.Data, unit * 2);
            color[3] = BitConverter.ToSingle(data.Data, unit * 3);
            l.color = new Color(color[0], color[1], color[2], color[3]);
        }
    }

    public override DataDetail UpdateData(GameObject obj)
    {
        DataDetail detail = new DataDetail();

        Light l = obj.GetComponent<Light>();
        if (l)
        {
            color[0] = l.color.r;
            color[1] = l.color.g;
            color[2] = l.color.b;
            color[3] = l.color.a;

            detail.Type = DataType.DataLightColor;
            detail.Data = color.ToByteArray();
        }

        return detail;
    }
}
