using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BattleCruisers.UI.BattleScene.ProgressBars;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
    public class SledgehammerCannon : OffenseMultiTurret
    {

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
        }
    }
}