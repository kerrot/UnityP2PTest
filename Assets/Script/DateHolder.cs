using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DateHolder : MonoBehaviour
{
    [SerializeField]
    private List<DataType> updateList = new List<DataType>();

    public int ID { get; set; }

    Dictionary<DataType, DataBase> dataMapping = new Dictionary<DataType, DataBase>();

    List<DataBase> data = new List<DataBase>();

    public List<DataBase> GetData()
    {
        data.Clear();

        updateList.ForEach(t =>
        {
            if (!dataMapping.ContainsKey(t))
            {
                string str = Enum.GetName(typeof(DataType), t);
                DataBase d = Activator.CreateInstance(Type.GetType(str)) as DataBase;
                dataMapping.Add(t, d);
            }

            dataMapping[t].UpdateData(gameObject);
            data.Add(dataMapping[t]);
        });

        return data;
    }
}
