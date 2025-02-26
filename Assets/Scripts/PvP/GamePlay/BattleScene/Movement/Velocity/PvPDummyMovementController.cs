using BattleCruisers.Movement.Velocity;
using System;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity
{
    public class PvPDummyMovementController : IMovementController
    {
        public Vector2 Velocity { get; set; }

        public event EventHandler<XDirectionChangeEventArgs> DirectionChanged { add { } remove { } }

        public void Activate() { }

        public void AdjustVelocity() { }
    }
}
