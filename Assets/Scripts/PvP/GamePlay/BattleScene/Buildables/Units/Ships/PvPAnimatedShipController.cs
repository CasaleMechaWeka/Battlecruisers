using BattleCruisers.Effects.Movement;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Ships
{
    public class PvPAnimatedShipController : PvPShipController
    {
        private ShipMovementEffect _movementEffects;
        public MovementEffectInitialiser movementEffectInitialiser;

        public override void StaticInitialise(GameObject parent, PvPHealthBarController healthBar)
        {
            base.StaticInitialise(parent, healthBar);

            Assert.IsNotNull(movementEffectInitialiser);
            _movementEffects = movementEffectInitialiser.CreateMovementEffects();
        }

        protected override void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();
            _movementEffects.Show();
        }

        protected override void OnBuildableCompleted_PvPClient()
        {

            base.OnBuildableCompleted_PvPClient();
            _movementEffects.Show();
        }

        protected override void StartMovementEffects()
        {
            _movementEffects.StartEffects();
            StartMovementEffectsOfClient();
        }

        protected override void StopMovementEffects()
        {
            _movementEffects.StopEffects();
            StopMovementEffectsOfClient();
        }

        protected override void Deactivate()
        {
            base.Deactivate();
            _movementEffects.ResetAndHide();
            ResetAndHideOfClient();
        }

        void StartMovementEffectsOfClient()
        {
            if (!IsHost)
            {
                if (IsClient)
                    _movementEffects.StartEffects();
            }
            else
                StartMovementEffectsClientRpc();
        }

        [ClientRpc]
        void StartMovementEffectsClientRpc()
        {
            if (!IsHost)
                StartMovementEffectsOfClient();
        }

        void StopMovementEffectsOfClient()
        {
            if (!IsHost)
            {
                if (IsClient)
                    _movementEffects.StopEffects();
            }
            else
                StopMovementEffectsClientRpc();
        }

        [ClientRpc]
        void StopMovementEffectsClientRpc()
        {
            if (!IsHost)
                StopMovementEffectsOfClient();
        }

        void ResetAndHideOfClient()
        {
            if (!IsHost)
            {
                if (IsClient)
                    _movementEffects.ResetAndHide();
            }
            else
                ResetHideClientRpc();
        }

        [ClientRpc]
        void ResetHideClientRpc()
        {
            if (!IsHost)
                ResetAndHideOfClient();
        }
    }
}
