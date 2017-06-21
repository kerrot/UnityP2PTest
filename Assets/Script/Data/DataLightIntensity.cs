using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DataLightIntensity : DataBase
{
    float intensity;

    public override void Apply(GameObject obj)
    {
        Light l = obj.GetComponent<Light>();
        if (l)
        {
            l.intensity = intensity;
        }
    }

    public override void UpdateData(GameObject obj)
    {
        Light l = obj.GetComponent<Light>();
        if (l)
        {
            intensity = l.intensity;
        }
    }
}
