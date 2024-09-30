using System;

namespace HuangD.Sessions.Maps;

public class Index
{
    public Index(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; init; }
    public int Y { get; init; }

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;
        var c = (Index)obj;
        if (c == null)
            return false;
        return X == c.X && Y == c.Y;
    }

    public override int GetHashCode()
    {
        return (X, Y).GetHashCode();
    }

    public int Distance(Index other)
    {
        return (int)(Math.Pow(other.X - X, 2) + Math.Pow(other.Y - Y, 2));
    }
}
