using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Utils;

namespace BattleCruisers.Buildables.Units.Ships
{
    public class DestroyerController : ShipController
	{
        private IBarrelWrapper _directFireAntiSea, _mortar, _directFireAntiAir, _samSite, _missileLauncher;

        private float _optimalArmamentRangeInM;
        protected override float OptimalArmamentRangeInM { get { return _optimalArmamentRangeInM; } }

        public override void StaticInitialise()
        {
            base.StaticInitialise();

            _attackCapabilities.Add(TargetType.Aircraft);
            _optimalArmamentRangeInM = FindOptimalArmamentRangeInM();
        }

        /// <summary>
        /// Want to:
        /// + Stay out of range of mortars
        /// + But go close enough for more than one destroyer to attack
        /// </summary>
        private float FindOptimalArmamentRangeInM()
        {
            // This is the range at which an enemy mortar will be able to attack :)
            return _mortar.RangeInM + Size.x / 2;
        }

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
            _directFireAntiAir = transform.FindNamedComponent<IBarrelWrapper>("DirectBurstFireAntiAir");
			turrets.Add(_directFireAntiAir);

            // SAM site
            _samSite = transform.FindNamedComponent<IBarrelWrapper>("SamSite");
            turrets.Add(_samSite);

            // Missile launcher
            _missileLauncher = transform.FindNamedComponent<IBarrelWrapper>("MissileLauncher");
            turrets.Add(_missileLauncher);

            return turrets;
		}

		protected override void OnInitialised()
		{
			base.OnInitialised();

			Faction enemyFaction = Helper.GetOppositeFaction(Faction);

			IList<TargetType> nonAirTargets = new List<TargetType>() { TargetType.Buildings, TargetType.Cruiser, TargetType.Ships };
            _directFireAntiSea.Initialise(this, _factoryProvider, enemyFaction, nonAirTargets);
            _mortar.Initialise(this, _factoryProvider, enemyFaction, nonAirTargets);
            _missileLauncher.Initialise(this, _factoryProvider, enemyFaction, nonAirTargets);

            IList<TargetType> airTargets = new List<TargetType>() { TargetType.Aircraft };
            _directFireAntiAir.Initialise(this, _factoryProvider, enemyFaction, airTargets);
            _samSite.Initialise(this, _factoryProvider, enemyFaction, airTargets);
		}
	}
}
