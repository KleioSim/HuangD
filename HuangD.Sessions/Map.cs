using DynamicData;
using System.ComponentModel;

namespace HuangD.Sessions;

public class Map
{
    public enum TerrainType
    {
        Water,
        Land,
        Mount,
        Steppe,
        Hill
    }

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
    }


    public SourceCache<TerrainItem, Index> Terrains = new SourceCache<TerrainItem, Index>(item => item.Index);

    public class TerrainItem
    {
        public Index Index { get; set; }
        public TerrainType Type { get; set; }
    }

    public Map()
    {
        Terrains.Edit(innerCache =>
        {
            innerCache.AddOrUpdate(new TerrainItem() { Index = new Index(0, 0), Type = TerrainType.Water });
            innerCache.AddOrUpdate(new TerrainItem() { Index = new Index(0, 1), Type = TerrainType.Land });
            innerCache.AddOrUpdate(new TerrainItem() { Index = new Index(0, 2), Type = TerrainType.Hill });
            innerCache.AddOrUpdate(new TerrainItem() { Index = new Index(0, 3), Type = TerrainType.Mount });
        });
    }
}