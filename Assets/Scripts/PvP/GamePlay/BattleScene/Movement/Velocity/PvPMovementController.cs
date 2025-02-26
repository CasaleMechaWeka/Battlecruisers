using BattleCruisers.Buildables.Units;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity.Providers;
using System;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.Utils.PlatformAbstractions.Time;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity
{
    public abstract class PvPMovementController : IMovementController
    {
        protected readonly IPvPVelocityProvider _maxVelocityProvider;
        protected readonly ITime _time;

        public abstract Vector2 Velocity { get; set; }

        public event EventHandler<XDirectionChangeEventArgs> DirectionChanged;

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
                Direction? newDirection = null;

                if (oldVelocity.x >= 0 && currentVelocity.x < 0)
                {
                    newDirection = Direction.Left;
                }
                else if (oldVelocity.x <= 0 && currentVelocity.x > 0)
                {
                    newDirection = Direction.Right;
                }

                if (newDirection != null)
                {
                    DirectionChanged.Invoke(this, new XDirectionChangeEventArgs((Direction)newDirection));
                }
            }
        }
    }
}
