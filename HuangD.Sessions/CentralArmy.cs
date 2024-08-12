namespace HuangD.Sessions;

public class CentralArmy
{
    public int Count { get; internal set; }
    public int ExpectCount { get; internal set; }
    public float Cost => ExpectCount / 1000;

    private Province Location { get; set; }

    public CentralArmy(int count, int expectCount)
    {
        Count = count;
        ExpectCount = expectCount;
    }
}