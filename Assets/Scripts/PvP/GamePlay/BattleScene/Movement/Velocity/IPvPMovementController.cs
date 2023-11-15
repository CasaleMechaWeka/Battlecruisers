using System;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity
{
    public class PvPXDirectionChangeEventArgs : EventArgs
    {
        public PvPDirection NewDirection { get; }

        public PvPXDirectionChangeEventArgs(PvPDirection newDirection)
        {
            NewDirection = newDirection;
        }
    }

    public interface IPvPMovementController
    {
        Vector2 Velocity { get; set; }

        event EventHandler<PvPXDirectionChangeEventArgs> DirectionChanged;

        void Activate();
        void AdjustVelocity();
    }
}
