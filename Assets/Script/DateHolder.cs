using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;
using UniRx;
using UniRx.Triggers;

public class DateHolder : UnityEngine.MonoBehaviour
{
    [SerializeField]
    private List<DataType> updateList = new List<DataType>();

    Dictionary<DataType, DataBase> dataMapping = new Dictionary<DataType, DataBase>();

    List<DataBase.DataDetail> data = new List<DataBase.DataDetail>();

    MonobitView view;

    private void Awake()
    {
        view = GetComponent<MonobitView>();
    }

    public List<DataBase.DataDetail> GetData()
    {
        data.Clear();

        updateList.ForEach(t =>
        {
            if (!dataMapping.ContainsKey(t))
            {
                string str = Enum.GetName(typeof(DataType), t);
                DataBase db = Activator.CreateInstance(Type.GetType(str)) as DataBase;
                dataMapping.Add(t, db);
            }

            DataBase.DataDetail detail = dataMapping[t].UpdateData(gameObject);
            data.Add(detail);
        });

        return data;
    }

    public void Apply(List<MonobitRPC.DataPackage> package)
    {
        package.ToObservable().Where(p => p.ID == view.viewID).Subscribe(p =>
        {
            p.Data.ForEach(d =>
            {
                if (!dataMapping.ContainsKey(d.Type))
                {
                    string str = Enum.GetName(typeof(DataType), d.Type);
                    DataBase db = Activator.CreateInstance(Type.GetType(str)) as DataBase;
                    dataMapping.Add(d.Type, db);
                }

                dataMapping[d.Type].Apply(gameObject, d);
            });
        });
    }
}
