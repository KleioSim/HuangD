using HuangD.Sessions.Utilties;

namespace HuangD.Sessions;

public class CentralArmy
{
    public string Key { get; }
    public int Count { get; internal set; }
    public int ExpectCount { get; internal set; }
    public float Cost => ExpectCount / 1000;

    public Country Owner { get; internal set; }
    public Province Position { get; internal set; }
    public MoveTo MoveTo { get; internal set; }

    public CentralArmy(int count, int expectCount, Country owner)
    {
        Key = UUID.Generate("ARMY");

        Count = count;
        ExpectCount = expectCount;
        Owner = owner;
        Position = owner.CapitalProvince;
    }
}

public class MoveTo
{
    public Province Target { get; }
}