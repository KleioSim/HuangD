namespace HuangD.Sessions.Maps;

public class MapCell
{
    public static IIndexMethods IndexMethods { get; set; } = new SquareIndexMethods();

    public Index Index { get; private init; }
    public int PopCount { get; private init; }
    public string ProvinceId { get; private init; }

    public TerrainType TerrainType { get; private init; }

    public MapCell(Index index, TerrainType terrainType, int popCount, string provinceId)
    {
        this.Index = index;
        this.TerrainType = terrainType;
        this.PopCount = popCount;
        this.ProvinceId = provinceId;
    }
}