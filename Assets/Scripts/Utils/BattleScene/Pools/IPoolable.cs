using System;
using BattleCruisers.Buildables;

namespace BattleCruisers.Utils.BattleScene.Pools
{
    public interface IPoolable<TArgs>
    {
        event EventHandler Deactivated;

        void Activate(TArgs activationArgs);

        void Activate(TArgs activationArgs, Faction faction);
    }
}