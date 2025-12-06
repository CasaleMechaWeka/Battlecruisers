using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Tactical.Shields;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Ships
{
    public class PvPTurtleController : PvPShipController
    {
        private PvPSectorShieldController _shieldController;

        private Animator animator;

        public override void StaticInitialise(GameObject parent, PvPHealthBarController healthBar)
        {
            base.StaticInitialise(parent, healthBar);

            _shieldController = GetComponentInChildren<PvPSectorShieldController>(includeInactive: true);
            animator = GetComponent<Animator>();
            Assert.IsNotNull(_shieldController, "Cannot find PvPSectorShieldController component");
            Assert.IsNotNull(animator, "Animator component could not be found.");
            animator.enabled = false; // Ensure the animator is disabled by default
            _shieldController.StaticInitialise();
        }

        public override void Activate(PvPBuildableActivationArgs activationArgs)
        {
            base.Activate(activationArgs);

            if (_shieldController != null)
            {
                _shieldController.Initialise(Faction, TargetType.Ships);
                _shieldController.gameObject.SetActive(false);
                OnEnableShieldClientRpc(false);
                _localBoosterBoostableGroup.AddBoostable(_shieldController.Stats);
                _localBoosterBoostableGroup.AddBoostProvidersList(_cruiserSpecificFactories.GlobalBoostProviders.ShieldRechargeRateBoostProviders);
            }
        }

        protected override void OnBuildableCompleted()
        {
            if (IsServer)
            {
                base.OnBuildableCompleted();
                _shieldController.gameObject.SetActive(true);
                _shieldController.ActivateShield();
                OnEnableShieldClientRpc(true);
                OnBuildableCompletedClientRpc();
            }
            else
            {
                OnBuildableCompleted_PvPClient();
            }
        }

        //-------------------------------------- RPCs -------------------------------------------------//

        [ClientRpc]
        private void OnEnableShieldClientRpc(bool enabled)
        {
            if (!IsHost)
                _shieldController.gameObject.SetActive(enabled);
        }
    }
}
