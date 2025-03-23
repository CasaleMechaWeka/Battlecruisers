using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Buildables.Units.Ships
{
    public class SiegeDestroyerController : ShipController
    {
        private IBarrelWrapper _mortar;

        private float _optimalArmamentRangeInM;
        public override float OptimalArmamentRangeInM => _optimalArmamentRangeInM;
        public override bool KeepDistanceFromEnemyCruiser => false;

        public override void StaticInitialise(GameObject parent, HealthBarController healthBar)
        {
            base.StaticInitialise(parent, healthBar);
            _optimalArmamentRangeInM = FindOptimalArmamentRangeInM();
        }

        /// <summary>
        /// Enemy detector is in ship center, but longest range barrel (mortar) is behind
        /// ship center.  Want to only stop once barrel is in range, so make optimal 
        /// armament range be less than the longest range barrel.
        private float FindOptimalArmamentRangeInM()
        {
            return _mortar.RangeInM - 0.2f;    //for safety margin
        }

        protected override IList<IBarrelWrapper> GetTurrets()
        {
            IList<IBarrelWrapper> turrets = new List<IBarrelWrapper>();

            // Mortar
            _mortar = transform.FindNamedComponent<IBarrelWrapper>("PrimaryWeapon");
            turrets.Add(_mortar);

            return turrets;
        }

        protected override void InitialiseTurrets()
        {
            _mortar.Initialise(this, _factoryProvider, _cruiserSpecificFactories, SoundKeys.Firing.BigCannon);
        }

        protected override void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();
            _mortar.ApplyVariantStats(this);
        }
    }
}
