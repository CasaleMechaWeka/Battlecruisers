using BattleCruisers.Effects.Movement;
using BattleCruisers.UI.BattleScene.ProgressBars;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Ships
{
    public abstract class AnimatedShipController : ShipController
	{
        private IMovementEffects _movementEffects;
        public MovementEffectInitialiser movementEffectInitialiser;

        public override void StaticInitialise(GameObject parent, HealthBarController healthBar)
        {
            base.StaticInitialise(parent, healthBar);
            
            Assert.IsNotNull(movementEffectInitialiser);
            _movementEffects = movementEffectInitialiser.CreateMovementEffects();
        }

        protected override void StartMovementEffects()
        {
            _movementEffects.StartEffects();
        }

        protected override void StopMovementEffects()
        {
            _movementEffects.StopEffects();
        }

        protected override void Deactivate()
        {
            base.Deactivate();
            _movementEffects.StopEffects();
        }
    }
}
