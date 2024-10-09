using System.Collections.Generic;
using System.Linq;

namespace HuangD.Sessions.Maps.Builders;

public static partial class MapBuilder
{
    public static IIndexMethods IndexMethods { get; set; } = new SquareIndexMethods();
}