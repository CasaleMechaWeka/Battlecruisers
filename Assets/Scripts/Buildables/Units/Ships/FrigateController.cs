using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Buildables.Units.Ships
{
    public class FrigateController : ShipController
    {
        private IBarrelWrapper _directFireAntiSea, _mortar, _samSite;// _directFireAntiAir;
        public override float OptimalArmamentRangeInM => 19;
        public override bool KeepDistanceFromEnemyCruiser => false;

        public override void StaticInitialise(GameObject parent, HealthBarController healthBar)
        {
            base.StaticInitialise(parent, healthBar);
        }

        /// <summary>
        /// Enemy detector is in ship center, but longest range barrel (mortar) is behind
        /// ship center.  Want to only stop once barrel is in range, so make optimal 
        /// armament range be less than the longest range barrel.

        protected override IList<IBarrelWrapper> GetTurrets()
        {
            IList<IBarrelWrapper> turrets = new List<IBarrelWrapper>();

            // Anti ship turret
            _directFireAntiSea = transform.FindNamedComponent<IBarrelWrapper>("DirectFireAntiSea");
            turrets.Add(_directFireAntiSea);

            // Mortar
            _mortar = transform.FindNamedComponent<IBarrelWrapper>("Mortar");
            turrets.Add(_mortar);

            // Anti air turret
            //_directFireAntiAir = transform.FindNamedComponent<IBarrelWrapper>("DirectBurstFireAntiAir");
            //turrets.Add(_directFireAntiAir);

            // SAM site
            _samSite = transform.FindNamedComponent<IBarrelWrapper>("SamSite");
            turrets.Add(_samSite);

            return turrets;
        }

        protected override void InitialiseTurrets()
        {
            _directFireAntiSea.Initialise(this, _cruiserSpecificFactories);
            _mortar.Initialise(this, _cruiserSpecificFactories);
            //_directFireAntiAir.Initialise(this, _factoryProvider, _cruiserSpecificFactories, SoundKeys.Firing.AntiAir);
            _samSite.Initialise(this, _cruiserSpecificFactories);
        }

        protected override void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();
            _samSite.ApplyVariantStats(this);
            _directFireAntiSea.ApplyVariantStats(this);
            _mortar.ApplyVariantStats(this);
        }
    }
}
