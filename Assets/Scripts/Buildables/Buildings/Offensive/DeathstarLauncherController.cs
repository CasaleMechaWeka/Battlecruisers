using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.UI.BattleScene.ProgressBars;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Offensive
{
    public class DeathstarLauncherController : SatelliteLauncherController
    {
        protected override Vector3 SpawnPositionAdjustment => new Vector3(0.003f, 0.21f, 0);
        public override TargetValue TargetValue => TargetValue.High;

        protected override void AddBuildRateBoostProviders(
            GlobalBoostProviders globalBoostProviders,
            IList<ObservableCollection<IBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(_cruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.UltrasProviders);
        }

        public override void StaticInitialise(GameObject parent, HealthBarController healthBar)
        {
            base.StaticInitialise(parent, healthBar);

            // Need satellite to be initialised to be able to access damage capabilities.
            satellitePrefab.StaticInitialise();

            foreach (IDamageCapability damageCapability in satellitePrefab.Buildable.DamageCapabilities)
            {
                AddDamageStats(damageCapability);
            }
        }
    }
}
