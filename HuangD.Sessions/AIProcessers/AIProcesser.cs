using Chrona.Engine.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HuangD.Sessions.AIProcessers;


public class AIProcesser
{
    private Session session;
    private Random random;

    public AIProcesser(Session session)
    {
        this.session = session;
    }

    internal void Run(AIDef def, IEnumerable<IEntity> entities)
    {
        foreach (var entity in entities)
        {
            if (!def.Triggers.All(x => x.IsVaild(entity, session)))
            {
                continue;
            }

            var factor = def.Factors.Sum(x => x.Calc(entity, session));
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
                var parameters = action.Parameters.Select(x => x.Get(entity, session)).ToArray();
                var command = contruct.Invoke(parameters) as IMessage;

                session.OnMessage(command);
            }
        }
    }
}

public interface AIDef
{
    public IEnumerable<ITrigger> Triggers { get; }
    public IEnumerable<IFactor> Factors { get; }
    public IEnumerable<IAction> Actions { get; }

    int CalcFactor(IEntity entity, ISessionData sessionData);
    bool IsVaild(IEntity entity, ISessionData sessionData);
}

public interface IAction
{
    public ConstructorInfo CommandDef { get; }
    public IEnumerable<IParameter> Parameters { get; }
}


public interface IParameter
{
    Func<IEntity, ISessionData, object> Get { get; }
}

public interface ITrigger
{
    bool IsVaild(IEntity entity, ISessionData session);
}

public interface IFactor
{
    int Calc(IEntity entity, ISessionData session);
}

public class CountryAIDef : AIDef
{
    public IEnumerable<ITrigger> Triggers => throw new NotImplementedException();

    public IEnumerable<IFactor> Factors => throw new NotImplementedException();

    public IEnumerable<IAction> Actions => throw new NotImplementedException();

    public int CalcFactor(IEntity entity, ISessionData sessionData)
    {
        throw new NotImplementedException();
    }

    public bool IsVaild(IEntity entity, ISessionData sessionData)
    {
        throw new NotImplementedException();
    }
}
