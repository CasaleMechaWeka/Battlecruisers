using System;

namespace BattleCruisers.Utils.BattleScene.Pools
{
    public interface IPoolable<TArgs>
    {
        event EventHandler Deactivated;

        void Activate(TArgs initialisationArgs);
    }
}