using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using System.Runtime.InteropServices;

public class DataLightIntensity : DataBase
{
    float intensity;

    public DataLightIntensity()
    {
        dataLength = Marshal.SizeOf(typeof(float));
    }

    public override void Apply(GameObject obj, List<byte> data)
    {
        Light l = obj.GetComponent<Light>();
        if (l)
        {
            intensity = BitConverter.ToSingle(data.ToArray(), 0);
            l.intensity = intensity;
        }
    }

    public override List<byte> UpdateData(GameObject obj)
    {
        data.Clear();

        Light l = obj.GetComponent<Light>();
        if (l && intensity != l.intensity)
        {
            intensity = l.intensity;

            data.Add((byte)DataType.DataLightIntensity);
            data.AddRange(BitConverter.GetBytes(intensity));
        }

        return data;
    }
}
