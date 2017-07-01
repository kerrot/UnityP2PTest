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
            CheckMappingExist(t);

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
                CheckMappingExist(d.Type);

                dataMapping[d.Type].Apply(gameObject, d);
            });
        });
    }

    private void CheckMappingExist(DataType type)
    {
        if (!dataMapping.ContainsKey(type))
        {
            string str = Enum.GetName(typeof(DataType), type);
            DataBase db = Activator.CreateInstance(Type.GetType(str)) as DataBase;
            dataMapping.Add(type, db);
        }
    }
}
