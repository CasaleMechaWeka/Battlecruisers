using BattleCruisers.Buildables;
using BattleCruisers.Utils.BattleScene.Pools;
using System;

namespace BattleCruisers.Effects
{
    public interface IDroneController : IPoolable<DroneActivationArgs>
    {
        Faction Faction { get; }

        event EventHandler Activated;

        void Deactivate();
    }
}