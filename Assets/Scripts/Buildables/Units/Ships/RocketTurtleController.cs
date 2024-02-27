using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;

namespace BattleCruisers.Buildables.Units.Ships
{
    public class RocketTurtleController : AnimatedShipController
    {
        private IBarrelWrapper _missileLauncher;

        public override float OptimalArmamentRangeInM => _missileLauncher.RangeInM;

        protected override IList<IBarrelWrapper> GetTurrets()
        {
            IList<IBarrelWrapper> turrets = new List<IBarrelWrapper>();

            _missileLauncher = gameObject.GetComponentInChildren<IBarrelWrapper>();
            Assert.IsNotNull(_missileLauncher);
            turrets.Add(_missileLauncher);

            return turrets;
        }

        protected override void InitialiseTurrets()
        {
            _missileLauncher.Initialise(this, _factoryProvider, _cruiserSpecificFactories, SoundKeys.Firing.RocketLauncher);
        }


        protected override void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();
            _missileLauncher.ApplyVariantStats(this);
        }
    }
}
