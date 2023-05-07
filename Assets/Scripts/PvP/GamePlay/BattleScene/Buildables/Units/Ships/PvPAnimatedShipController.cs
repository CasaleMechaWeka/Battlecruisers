using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Movement;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Utils.Localisation;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Ships
{
    public abstract class PvPAnimatedShipController : PvPShipController
    {
        private IPvPMovementEffect _movementEffects;
        public PvPMovementEffectInitialiser movementEffectInitialiser;

        public override void StaticInitialise(GameObject parent, PvPHealthBarController healthBar, ILocTable commonStrings)
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
