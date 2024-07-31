using System.Collections.Generic;

namespace HuangD.Sessions;

public partial class Province
{
    public Province(string key)
    {
        Key = key;
    }

    public string Key { get; }

    public IEnumerable<Province> Neighbors { get; private set; } = new Province[] { };
}