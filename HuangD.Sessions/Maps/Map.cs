﻿using DynamicData;

namespace HuangD.Sessions.Maps;

public partial class Map
{
    public static IIndexMethods IndexMethods { get; set; } = new SquareIndexMethods();

    public SourceCache<TerrainItem, Index> Terrains = new SourceCache<TerrainItem, Index>(item => item.Index);
}