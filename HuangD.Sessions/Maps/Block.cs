using System.Collections.Generic;

namespace HuangD.Sessions.Maps;

public class Block
{
    public string Id { get; set; }
    public Index coreIndex { get; set; }
    public HashSet<Index> Edges { get; set; } = new HashSet<Index>();
    public List<Index> InvaildEdges { get; set; } = new List<Index>();
    public HashSet<Index> Indexes { get; set; } = new HashSet<Index>();
    public HashSet<Block> Neighbors { get; set; } = new HashSet<Block>();
}