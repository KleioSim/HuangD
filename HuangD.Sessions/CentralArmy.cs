namespace HuangD.Sessions;

public class CentralArmy
{
    public int Count { get; internal set; }
    public int ExpectCount { get; internal set; }
    public float Cost => ExpectCount / 1000;

    public Province Location { get; internal set; }
    public MoveTo MoveTo { get; internal set; }

    public CentralArmy(int count, int expectCount)
    {
        Count = count;
        ExpectCount = expectCount;
    }
}

public class MoveTo
{
    public Province Target { get; }
}