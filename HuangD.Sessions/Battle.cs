using Chrona.Engine.Core.Interfaces;
using HuangD.Sessions.Messages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Sessions;

public class Battle
{
    public Province Province { get; }

    public IEnumerable<CentralArmy> OffenseArmy => Province.centralArmies.Where(x => x.Owner != Province.Owner);
    public IEnumerable<CentralArmy> DefenseCentralArmy => Province.centralArmies.Where(x => x.Owner == Province.Owner);
    public LocalArmy DefenseLocalArmy => Province.LocalArmy;
    public IEnumerable<BattleReport> BattleReports => battleReports;

    private List<BattleReport> battleReports = new List<BattleReport>();

    public Battle(Province province)
    {
        Province = province;
    }

    public BattleReport[] OnNextTurn(Date date)
    {
        List<BattleReport> reports = new List<BattleReport>();

        var random = new Random();
        var randomValue = random.Next(1, 11);

        if (randomValue <= 5)
        {
            reports.Add(new BattleReport()
            {
                OffenseArmy = OffenseArmy.ToArray(),
                DefenseCentralArmy = DefenseCentralArmy.ToArray(),
                DefenseLocalArmy = DefenseLocalArmy,
                Date = (date.Year, date.Month, date.Day),
                Desc = $"Battle, Province:{Province.Id}, Offense:[{string.Join(",", OffenseArmy.Select(x => x.Id))}], Defense[{string.Join(",", DefenseCentralArmy.Select(x => x.Id).Append(DefenseLocalArmy.Id))}], Defense Failed"
            });

            if (randomValue <= 2)
            {
                randomValue = random.Next(1, 11);

                if (randomValue <= 5)
                {
                    IEntity.SendMessage(new Command_ChangeProvinceOwner(Province.Id, OffenseArmy.First().Owner.Id));
                }
            }
        }
        else
        {
            reports.Add(new BattleReport()
            {
                OffenseArmy = OffenseArmy.ToArray(),
                DefenseCentralArmy = DefenseCentralArmy.ToArray(),
                DefenseLocalArmy = DefenseLocalArmy,
                Date = (date.Year, date.Month, date.Day),
                Desc = $"Battle, Province:{Province.Id}, Offense:[{string.Join(",", OffenseArmy.Select(x => x.Id))}], Defense[{string.Join(",", DefenseCentralArmy.Select(x => x.Id).Append(DefenseLocalArmy.Id))}], Defense Success"
            });

            if (randomValue >= 8)
            {
            }
        }

        battleReports.AddRange(reports);
        return reports.ToArray();
    }
}

public class BattleReport
{
    public CentralArmy[] OffenseArmy { get; init; }
    public CentralArmy[] DefenseCentralArmy { get; init; }
    public LocalArmy DefenseLocalArmy { get; init; }
    public (int year, int month, int day) Date { get; init; }
    public string Desc { get; init; }

    public override string ToString()
    {
        return Desc;
    }
}