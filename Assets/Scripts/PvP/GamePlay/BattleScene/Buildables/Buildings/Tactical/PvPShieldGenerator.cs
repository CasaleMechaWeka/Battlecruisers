using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Netcode;
using BattleCruisers.Buildables;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Tactical.Shields
{
    public class PvPShieldGenerator : PvPTacticalBuilding
    {
        private PvPShieldController _shieldController;
        public override TargetValue TargetValue => TargetValue.Medium;
        public override bool IsBoostable => true;

        public override void StaticInitialise(GameObject parent, PvPHealthBarController healthBar)
        {
            base.StaticInitialise(parent, healthBar);

            _shieldController = GetComponentInChildren<PvPShieldController>(includeInactive: true);
            Assert.IsNotNull(_shieldController);
            _shieldController.StaticInitialise();
        }

        public override void Activate(PvPBuildingActivationArgs activationArgs)
        {
            base.Activate(activationArgs);

            _shieldController.Initialise(Faction);
            _shieldController.gameObject.SetActive(false);
            OnEnableShieldClientRpc(false);
            _localBoosterBoostableGroup.AddBoostable(_shieldController.Stats);
            _localBoosterBoostableGroup.AddBoostProvidersList(_cruiserSpecificFactories.GlobalBoostProviders.ShieldRechargeRateBoostProviders);
        }

        protected override void OnBuildableCompleted()
        {
            if (IsServer)
            {
                base.OnBuildableCompleted();
                _shieldController.gameObject.SetActive(true);
                OnEnableShieldClientRpc(true);
                OnBuildableCompletedClientRpc();
            }
            else
                OnBuildableCompleted_PvPClient();
        }

        [ClientRpc]
        private void OnBuildableCompletedClientRpc()
        {
            if (!IsHost)
                OnBuildableCompleted();
            _shieldController.ApplyVariantStats(this);
        }

        [ClientRpc]
        private void OnEnableShieldClientRpc(bool enabled)
        {
            if (!IsHost)
                _shieldController.gameObject.SetActive(enabled);
        }
    }
}
