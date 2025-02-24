using BattleCruisers.Buildables.Units;
using System;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity
{
    public class PvPXDirectionChangeEventArgs : EventArgs
    {
        public Direction NewDirection { get; }

        public PvPXDirectionChangeEventArgs(Direction newDirection)
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
