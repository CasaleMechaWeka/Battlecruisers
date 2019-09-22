using BattleCruisers.Utils.BattleScene.Pools;
using System;
using UnityEngine;

namespace BattleCruisers.Effects
{
    public interface IDroneController : IPoolable<Vector2>
    {
        event EventHandler Activated;

        void Deactivate();
    }
}