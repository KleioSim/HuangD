namespace HuangD.Sessions.Maps.Builders;

public static class PopCountBuilder
{
    public static Dictionary<Index, int> Build(Dictionary<Index, TerrainType> terrainDict, string seed)
    {
        var random = new Random();
        var baseValueDict = terrainDict.Where(x => x.Value != TerrainType.Water).ToDictionary(k => k.Key, v =>
        {
            int popCount = 0;

            var index = v.Key;
            var terrain = v.Value;

            switch (terrain)
            {
                case TerrainType.Land:
                    popCount = random.Next(1000, 5000);
                    break;
                case TerrainType.Hill:
                    popCount = random.Next(100, 500);
                    break;
                case TerrainType.Mount:
                    popCount = random.Next(10, 50);
                    break;
            }


            popCount += Map.IndexMethods.GetNeighborCells(index).Values.Select(neighbor =>
            {
                if (terrainDict.TryGetValue(neighbor, out TerrainType neighborTerrain))
                {
                    switch (neighborTerrain)
                    {
                        case TerrainType.Land:
                            return random.Next(500, 1000);
                        case TerrainType.Hill:
                            return random.Next(50, 100);
                        case TerrainType.Mount:
                            return 0;
                    }
                }

                return 0;
            }).Sum();

            return popCount;
        });

        return baseValueDict.ToDictionary(k => k.Key, v =>
        {
            var currIndex = v.Key;
            var currValue = v.Value;

            var neighorValues = Map.IndexMethods.GetNeighborCells(currIndex).Values
                .Where(neighbor => baseValueDict.ContainsKey(neighbor))
                .Select(neighbor => baseValueDict[neighbor]);

            return currValue * Math.Min(1, (int)(neighorValues.Average()) / 1000);
        });
    }
}