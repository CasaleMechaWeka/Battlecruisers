using BattleCruisers.Utils.BattleScene.Pools;
using System;
using UnityEngine;

namespace BattleCruisers.Effects
{
    public interface IDroneController : IPoolable<DroneActivationArgs>
    {
        event EventHandler Activated;

        void Deactivate();
    }
}