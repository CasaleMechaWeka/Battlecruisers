using BattleCruisers.Effects.Movement;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.Utils.Localisation;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Ships
{
    public abstract class AnimatedShipController : ShipController
	{
        private IMovementEffect _movementEffects;
        public MovementEffectInitialiser movementEffectInitialiser;

        public override void StaticInitialise(GameObject parent, HealthBarController healthBar, ILocTable commonStrings)
        {
            base.StaticInitialise(parent, healthBar, commonStrings);
            
            Assert.IsNotNull(movementEffectInitialiser);
            _movementEffects = movementEffectInitialiser.CreateMovementEffects();
        }

        protected override void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();
            _movementEffects.Show();
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
            _movementEffects.ResetAndHide();
        }
    }
}
