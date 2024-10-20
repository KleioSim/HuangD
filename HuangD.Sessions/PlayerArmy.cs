using Chrona.Engine.Core.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Sessions;

internal class PlayerArmy : IEntity
{
    private Session session;

    public string Id { get; } = nameof(PlayerArmy);
    public IEnumerable<LocalArmy> localArmies => session.PlayerCountry.Provinces.Select(x => x.LocalArmy);

    public PlayerArmy(Session session)
    {
        this.session = session;
    }

}