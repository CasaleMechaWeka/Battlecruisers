using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Ships
{
	public class FrigateController : ShipController
	{
        private IBarrelWrapper _directFireAntiSea;



		// FELIX
		public override float Damage { get { return 0; } }
		// FELIX
		protected override float EnemyDetectionRangeInM { get { return 5; } }

		public override void StaticInitialise()
		{
			base.StaticInitialise();

            _attackCapabilities.Add(TargetType.Aircraft);

            // Anti ship turret
            _directFireAntiSea = transform.Find("DirectFireAntiSea").gameObject.GetComponent<IBarrelWrapper>();
            Assert.IsNotNull(_directFireAntiSea);
            _directFireAntiSea.StaticInitialise();

            // Mortar

            // SAM site
		}

		protected override void OnInitialised()
		{
			base.OnInitialised();

			Faction enemyFaction = Helper.GetOppositeFaction(Faction);
            IList<TargetType> nonAirTargets = new List<TargetType>() { TargetType.Buildings, TargetType.Cruiser, TargetType.Ships };

            // Anti ship turret
            _directFireAntiSea.Initialise(_factoryProvider, enemyFaction, nonAirTargets);

            // Mortar

            // SAM site
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			// FELIX  Turrets :D
            _directFireAntiSea.StartAttackingTargets();
		}
	}
}
