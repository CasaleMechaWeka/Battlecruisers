using System;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity
{
    public class PvPDummyMovementController : IPvPMovementController
    {
        public Vector2 Velocity { get; set; }

        public event EventHandler<PvPXDirectionChangeEventArgs> DirectionChanged { add { } remove { } }

        public void Activate() { }

        public void AdjustVelocity() { }
    }
}
