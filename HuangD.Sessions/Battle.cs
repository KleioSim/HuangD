using System.Collections.Generic;
using System.Linq;

namespace HuangD.Sessions;

public class Battle
{
    public Province Province { get; }

    public IEnumerable<Army> Offense => Province.centralArmies.Where(x => x.Owner != Province.Owner);
    public IEnumerable<Army> Defense => Province.centralArmies.Where(x => x.Owner == Province.Owner).OfType<Army>().Append(Province.LocalArmy);

    public Battle(Province province)
    {
        Province = province;
    }
}