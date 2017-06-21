using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DataLightColor : DataBase
{
    [Serializable]
    public struct CustomColor
    {
        public float a;
        public float b;
        public float g;
        public float r;
    }

    CustomColor color;

    public override void Apply(GameObject obj)
    {
        Light l = obj.GetComponent<Light>();
        if (l)
        {
            l.color = new Color(color.r, color.g, color.b, color.a);
        }
    }

    public override void UpdateData(GameObject obj)
    {
        Light l = obj.GetComponent<Light>();
        if (l)
        {
            color.r = l.color.r;
            color.g = l.color.g;
            color.b = l.color.b;
            color.a = l.color.a;
        }
    }
}
