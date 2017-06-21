using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public abstract class DataBase
{
    public abstract void Apply(GameObject obj);
    public abstract void UpdateData(GameObject obj);
}
