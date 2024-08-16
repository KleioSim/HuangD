using System.Collections.Generic;
using System.Linq;

namespace HuangD.Sessions;

public class Economy
{
    public float Reserve { get; }
    public float Income => PopTaxes.Sum(x => x.Current);
    public float Spend => owner.CenterArmies.Sum(x => x.Count);

    public float Surplus => Income - Spend;

    public IEnumerable<PopTax> PopTaxes => owner.Provinces.Select(x => x.PopTax);
    public IEnumerable<CentralArmy> ArmyCost => owner.CenterArmies;

    private Country owner;

    public Economy(Country country)
    {
        this.owner = country;
    }
}