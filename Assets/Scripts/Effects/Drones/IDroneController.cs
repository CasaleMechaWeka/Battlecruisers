using BattleCruisers.Buildables;
using BattleCruisers.Utils.BattleScene.Pools;
using System;

namespace BattleCruisers.Effects.Drones
{
    public interface IDroneController : IPoolable<DroneActivationArgs>
    {
        Faction Faction { get; }

        event EventHandler Activated;

        void Deactivate();
    }
}