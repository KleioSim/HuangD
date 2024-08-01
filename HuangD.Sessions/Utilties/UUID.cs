

using System;
using System.Collections.Generic;

namespace HuangD.Sessions.Utilties;

internal class UUID
{
    private static Dictionary<string, ushort> dictCount = new Dictionary<string, ushort>();

    internal static string Generate(string key)
    {
        if (!dictCount.ContainsKey(key))
        {
            dictCount.Add(key, 0);
        }

        if (dictCount[key] == ushort.MaxValue)
        {
            throw new Exception();
        }

        dictCount[key]++;

        return key + string.Format("{0:X4}", dictCount[key]);
    }
}
