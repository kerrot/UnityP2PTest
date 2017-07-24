using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;
using System.Runtime.Serialization; // DataContractSerializer
using System.IO;

public static class Extensions
{
    public static void ChangeEach<T>(this T[] array, Func<T, T> mutator)
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = mutator(array[i]);
        }
    }
    public static byte[] ToByteArray<T>(this T[] array)
    {
        byte[] result = new byte[array.Length * Marshal.SizeOf(typeof(T))];
        Buffer.BlockCopy(array, 0, result, 0, result.Length);
        return result;
    }

    //public static T DeepClone<T>(this T obj)
    //{
    //    using (var ms = new MemoryStream())
    //    {
    //        var formatter = new BinaryFormatter();
    //        formatter.Serialize(ms, obj);
    //        ms.Position = 0;

    //        return (T)formatter.Deserialize(ms);
    //    }
    //}
}
