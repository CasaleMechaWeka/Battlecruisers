using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Ships
{
    public class DestroyerController : ShipController
	{
        private IBarrelWrapper _directFireAntiSea, _mortar, _directFireAntiAir, _samSite;

        protected override float EnemyDetectionRangeInM { get { return _mortar.TurretStats.rangeInM; } }

        public override void StaticInitialise()
        {
            base.StaticInitialise();
            _attackCapabilities.Add(TargetType.Aircraft);
        }

        protected override IList<IBarrelWrapper> GetTurrets()
        {
            IList<IBarrelWrapper> turrets = new List<IBarrelWrapper>();

            // Anti ship turret
            _directFireAntiSea = transform.Find("DirectFireAntiSea").gameObject.GetComponent<IBarrelWrapper>();
            Assert.IsNotNull(_directFireAntiSea);
            turrets.Add(_directFireAntiSea);

            // Mortar
            _mortar = transform.Find("Mortar").gameObject.GetComponent<IBarrelWrapper>();
            Assert.IsNotNull(_mortar);
            turrets.Add(_mortar);

            // Anti air turret
            _directFireAntiAir = transform.Find("DirectBurstFireAntiAir").gameObject.GetComponent<IBarrelWrapper>();
            Assert.IsNotNull(_directFireAntiAir);
			turrets.Add(_directFireAntiAir);

            // SAM site
            _samSite = transform.Find("SamSite").gameObject.GetComponent<IBarrelWrapper>();
            Assert.IsNotNull(_samSite);
            turrets.Add(_samSite);

            // Missile launcher
            // FELIX

            return turrets;
		}

		protected override void OnInitialised()
		{
			base.OnInitialised();

			Faction enemyFaction = Helper.GetOppositeFaction(Faction);

			IList<TargetType> nonAirTargets = new List<TargetType>() { TargetType.Buildings, TargetType.Cruiser, TargetType.Ships };
            _directFireAntiSea.Initialise(_factoryProvider, enemyFaction, nonAirTargets);
            _mortar.Initialise(_factoryProvider, enemyFaction, nonAirTargets);

            IList<TargetType> airTargets = new List<TargetType>() { TargetType.Aircraft };
            _directFireAntiAir.Initialise(_factoryProvider, enemyFaction, airTargets);
            _samSite.Initialise(_factoryProvider, enemyFaction, airTargets);
		}
	}
}
