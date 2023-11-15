using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity.Providers;
using System;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity
{
    public abstract class PvPMovementController : IPvPMovementController
    {
        protected readonly IPvPVelocityProvider _maxVelocityProvider;
        protected readonly IPvPTime _time;

        public abstract Vector2 Velocity { get; set; }

        public event EventHandler<PvPXDirectionChangeEventArgs> DirectionChanged;

        protected PvPMovementController(IPvPVelocityProvider maxVelocityProvider)
        {
            Assert.IsTrue(maxVelocityProvider.VelocityInMPerS > 0);
            _maxVelocityProvider = maxVelocityProvider;
            _time = PvPTimeBC.Instance;
        }

        public virtual void Activate() { }

        public abstract void AdjustVelocity();

        protected void HandleDirectionChange(Vector2 oldVelocity, Vector2 currentVelocity)
        {
            if (DirectionChanged != null)
            {
                PvPDirection? newDirection = null;

                if (oldVelocity.x >= 0 && currentVelocity.x < 0)
                {
                    newDirection = PvPDirection.Left;
                }
                else if (oldVelocity.x <= 0 && currentVelocity.x > 0)
                {
                    newDirection = PvPDirection.Right;
                }

                if (newDirection != null)
                {
                    DirectionChanged.Invoke(this, new PvPXDirectionChangeEventArgs((PvPDirection)newDirection));
                }
            }
        }
    }
}
