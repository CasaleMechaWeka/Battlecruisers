using BattleCruisers.Buildables.Pools;
using BattleCruisers.UI.BattleScene.ProgressBars;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Tactical.Shields
{
    public class UltraShieldGenerator : Building
    {
        private ShieldController _shieldController;

        public override TargetValue TargetValue => TargetValue.Medium;
        public override bool IsBoostable => true;

        public override void StaticInitialise(GameObject parent, HealthBarController healthBar)
        {
            base.StaticInitialise(parent, healthBar);

            _shieldController = GetComponentInChildren<ShieldController>(includeInactive: true);
            Assert.IsNotNull(_shieldController);
            _shieldController.StaticInitialise();
        }

        public override void Activate(BuildingActivationArgs activationArgs)
        {
            base.Activate(activationArgs);

            _shieldController.Initialise(Faction);
            _shieldController.gameObject.SetActive(false);

            _localBoosterBoostableGroup.AddBoostable(_shieldController.Stats);
            _localBoosterBoostableGroup.AddBoostProvidersList(_cruiserSpecificFactories.GlobalBoostProviders.ShieldRechargeRateBoostProviders);
        }

        protected override void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();

            _shieldController.gameObject.SetActive(true);
            _shieldController.ApplyVariantStats(this);
        }
    }
}
