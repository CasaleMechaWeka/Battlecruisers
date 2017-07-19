using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Offensive
{
	public class NukeLauncherController : Building
	{
		private NukeSpinner _spinner;
		private NukeController _nukeMissile;
		private NukeStats _nukeMissileStats;

		public SiloHalfController leftSiloHalf, rightSiloHalf;
		public NukeController nukeMissilePrefab;

		private const float SILO_HALVES_ROTATE_SPEED_IN_M_PER_S = 15;
		private const float SILO_TARGET_ANGLE_IN_DEGREES = 45;
		private static Vector3 NUKE_SPAWN_POSITION_ADJUSTMENT = new Vector3(0, -0.3f, 0);

		public override TargetValue TargetValue { get { return TargetValue.High; } }

		public override void StaticInitialise()
		{
			base.StaticInitialise();

			Assert.IsNotNull(leftSiloHalf);
			Assert.IsNotNull(rightSiloHalf);
			Assert.IsNotNull(nukeMissilePrefab);

			leftSiloHalf.StaticInitialise();
			rightSiloHalf.StaticInitialise();

			_spinner = gameObject.GetComponentInChildren<NukeSpinner>();
			Assert.IsNotNull(_spinner);
			_spinner.StaticInitialise();

            _nukeMissileStats = new NukeStats(nukePrefab: nukeMissilePrefab, damage: 20000, maxVelocityInMPerS: 10, cruisingAltitudeInM: 30, damageRadiusInM: 10);
		}

		protected override void OnInitialised()
		{
			base.OnInitialised();

			leftSiloHalf.Initialise(_movementControllerFactory, SILO_HALVES_ROTATE_SPEED_IN_M_PER_S, SILO_TARGET_ANGLE_IN_DEGREES);
			rightSiloHalf.Initialise(_movementControllerFactory, SILO_HALVES_ROTATE_SPEED_IN_M_PER_S, SILO_TARGET_ANGLE_IN_DEGREES);

			_spinner.Initialise(_movementControllerFactory);
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			// FELIX  Spinner is never visible :(
			_spinner.StartRotating();
			_spinner.StopRotating();
			_spinner.Renderer.enabled = false;

			CreateNuke();

			leftSiloHalf.ReachedDesiredAngle += SiloHalf_ReachedDesiredAngle;

			leftSiloHalf.StartRotating();
			rightSiloHalf.StartRotating();
		}

		private void CreateNuke()
		{
			_nukeMissile = Instantiate<NukeController>(nukeMissilePrefab);
			_nukeMissile.transform.position = transform.position + NUKE_SPAWN_POSITION_ADJUSTMENT;

			ITargetFilter targetFilter = _factoryProvider.TargetsFactory.CreateExactMatchTargetFilter(_enemyCruiser);
            ITargetFilter damageTargetFilter = _factoryProvider.TargetsFactory.CreateDummyTargetFilter(isMatchResult: true);
			IDamageApplier damageApplier = new AreaOfEffectDamageApplier(_nukeMissileStats.Damage, _nukeMissileStats.DamageRadiusInM, damageTargetFilter);
            IFlightPointsProvider flightPointsProvider = _factoryProvider.FlightPointsProviderFactory.NukeFlightPointsProvider;

			_nukeMissile.Initialise(_nukeMissileStats, _nukeMissileStats.InitialVelocityInMPerS, targetFilter, damageApplier, _enemyCruiser, _movementControllerFactory, flightPointsProvider);
		}

		private void SiloHalf_ReachedDesiredAngle(object sender, EventArgs e)
		{
			leftSiloHalf.ReachedDesiredAngle -= SiloHalf_ReachedDesiredAngle;
			_nukeMissile.Launch();
		}

		protected override void EnableRenderers(bool enabled)
		{
			base.EnableRenderers(enabled);

			_spinner.Renderer.enabled = enabled;
			leftSiloHalf.Renderer.enabled = enabled;
			rightSiloHalf.Renderer.enabled = enabled;
		}
	}
}
