using Chrona.Engine.Core.Interfaces;
using HuangD.Sessions.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static System.Formats.Asn1.AsnWriter;

namespace HuangD.Sessions.AIProcessers;


public class AIProcesser
{
    private Session session;
    private Random random;

    public AIProcesser(Session session)
    {
        this.session = session;
        random = new Random();
    }

    internal void Run(AIDef def, IEnumerable<IEntity> entities)
    {
        foreach (var entity in entities)
        {
            var context = new Context(entity, session);
            if (!def.Triggers.All(x => x.IsTrue(context)))
            {
                continue;
            }

            var factor = def.Factors.Where(x => x.Conditon.IsTrue(context)).Sum(x => x.Value);
            if (factor <= 0)
            {
                continue;
            }

            if (factor < 100 && random.Next(1, 101) > factor)
            {
                continue;
            }

            foreach (var action in def.Actions)
            {
                var contruct = action.CommandDef;
                var parameters = action.Parameters.Select(x => GetParameter(x, context)).ToArray();
                var command = contruct.Invoke(parameters) as IMessage;

                session.OnMessage(command);
            }
        }
    }

    private object GetParameter(IParameter paramDef, Context context)
    {
        var scope = paramDef.Scope.Get(context);

        var list = new List<(float factor, object item)>();
        foreach (var item in scope)
        {
            var factorValue = 0f;
            if (paramDef.Factors != null)
            {
                factorValue = paramDef.Factors.Where(x => x.Conditon.IsTrue(context))
                    .Sum(x => x.Value);
            }

            list.Add((factorValue, item));
        }

        return list.MaxBy(x => x.factor).item;
    }
}

internal class Context : IContext
{

    public IEntity Entity { get; }

    public ISessionData Session { get; }

    public Context(IEntity entity, Session session)
    {
        this.Entity = entity;
        this.Session = session;
    }
}

public interface IContext
{
    public IEntity Entity { get; }
    public ISessionData Session { get; }
}

public interface AIDef
{
    public IEnumerable<IConditon> Triggers { get; }
    public IEnumerable<IFactor> Factors { get; }
    public IEnumerable<IAction> Actions { get; }
}

public interface IAction
{
    public ConstructorInfo CommandDef { get; }
    public IEnumerable<IParameter> Parameters { get; }
}


public interface IParameter
{
    IScope Scope { get; }
    IEnumerable<IFactor> Factors { get; }
}

public interface IScope
{
    IEnumerable<object> Get(IContext context);
}

public interface IConditon
{
    bool IsTrue(IContext context);
}

public interface IFactor
{
    IConditon Conditon { get; }
    float Value { get; }
}

public interface ICountryAIDef : AIDef
{
}

public class CountryDeclareWar : ICountryAIDef
{
    public IEnumerable<IConditon> Triggers { get; } = new[]
    {
        new NotCondition(new IsInWar())
    };

    public IEnumerable<IFactor> Factors { get; } = new[]
    {
        new RandomFactor(10)
    };

    public IEnumerable<IAction> Actions { get; } = new[]
    {
        new Action()
        {
            CommandDef = typeof(Command_WarStart).GetConstructors().First(),
            Parameters = new []
            {
                new Parameter()
                {
                    Scope = new ScopeSelf()
                },

                new Parameter()
                {
                    Scope = new ScopeNeighborCounties()
                }
            }
        }
    };
}

internal class ScopeNeighborCounties : IScope
{
    public IEnumerable<object> Get(IContext context)
    {
        var country = context.Entity as Country;
        return country.Neighbors;
    }
}

internal class ScopeSelf : IScope
{
    public IEnumerable<object> Get(IContext context)
    {
        return new[] { context.Entity };
    }
}

public class Parameter : IParameter
{
    public IScope Scope { get; init; }

    public IEnumerable<IFactor> Factors { get; init; }
}

public class Action : IAction
{
    public ConstructorInfo CommandDef { get; init; }

    public IEnumerable<IParameter> Parameters { get; init; }
}

internal class NotCondition : IConditon
{
    private IConditon condition;

    public NotCondition(IConditon condition)
    {
        this.condition = condition;
    }

    public bool IsTrue(IContext context)
    {
        return !condition.IsTrue(context);
    }
}

public class IsInWar : IConditon
{
    public bool IsTrue(IContext context)
    {
        var country = context.Entity as Country;
        return country.Wars.Any();
    }
}

public class RandomFactor : IFactor
{
    public IConditon Conditon { get; } = new TrueCondtion();

    public float Value { get; }

    public RandomFactor(float value)
    {
        this.Value = value;
    }
}

public class TrueCondtion : IConditon
{
    public bool IsTrue(IContext context)
    {
        return true;
    }
}