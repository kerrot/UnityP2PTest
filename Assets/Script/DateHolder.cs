using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;
using UniRx;
using UniRx.Triggers;

using System.Runtime.InteropServices;

public class DateHolder : UnityEngine.MonoBehaviour
{
    [SerializeField]
    private List<DataType> updateList = new List<DataType>();

    public int ID { get { return view ? view.viewID : 0; } }

    Dictionary<DataType, DataBase> dataMapping = new Dictionary<DataType, DataBase>();

    int BASE_SIZE = Marshal.SizeOf(typeof(int));
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

        if (data.Count > BASE_SIZE)
        {
            return data;
        }
        else
        {
            return null;
        }
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
