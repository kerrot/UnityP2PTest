using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class DataLightIntensity : DataBase
{
    float intensity;

    public override void Apply(GameObject obj, DataDetail data)
    {
        Light l = obj.GetComponent<Light>();
        if (l)
        {
            l.intensity = BitConverter.ToSingle(data.Data, 0);
        }
    }

    public override DataDetail UpdateData(GameObject obj)
    {
        DataDetail detail = new DataDetail();

        Light l = obj.GetComponent<Light>();
        if (l)
        {
            intensity = l.intensity;
            detail.Type = DataType.DataLightIntensity;
            detail.Data = BitConverter.GetBytes(intensity);
        }

        return detail;
    }
}
