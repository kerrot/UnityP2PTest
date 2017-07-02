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

    public int ID { get { return view ? view.viewID : 0; } }

    Dictionary<DataType, DataBase> dataMapping = new Dictionary<DataType, DataBase>();

    List<byte> data = new List<byte>();

    MonobitView view;

    private void Awake()
    {
        view = GetComponent<MonobitView>();
    }

    public List<byte> GetData()
    {
        data.Clear();
        data.AddRange(BitConverter.GetBytes(view.viewID));

        updateList.ForEach(t =>
        {
            CheckMappingExist(t);

            data.AddRange(dataMapping[t].UpdateData(gameObject));
        });

        return data;
    }

    public void Apply(List<byte> package)
    {
        if (package != null && package.Count > 0)
        {
            int count = 0;

            do
            {
                DataType type = (DataType)package[count];
                CheckMappingExist(type);

                DataBase db = dataMapping[type];
                if (package.Count >= count + 1 + db.DataLength)
                {
                    db.Apply(gameObject, package.GetRange(count + 1, db.DataLength));
                }

                count += db.DataLength + 1;
            } while (package.Count > count);
        }


        //package.ToObservable().Where(p => p.ID == view.viewID).Subscribe(p =>
        //{
        //    p.Data.ForEach(d =>
        //    {
        //        CheckMappingExist(d.Type);

        //        dataMapping[d.Type].Apply(gameObject, d);
        //    });
        //});
    }

    private void CheckMappingExist(DataType type)
    {
        if (!dataMapping.ContainsKey(type))
        {
            string str = Enum.GetName(typeof(DataType), type);
            if (str != null)
            {
                DataBase db = Activator.CreateInstance(Type.GetType(str)) as DataBase;
                dataMapping.Add(type, db);
            }
        }
    }
}
