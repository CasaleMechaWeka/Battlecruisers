using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Data.Static;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Offensive
{
    public class NukeLauncherController : Building
	{
		private NukeSpinner _spinner;
		private INukeStats _nukeStats;
        private NukeController _launchedNuke;

		public SiloHalfController leftSiloHalf, rightSiloHalf;
		public NukeController nukeMissilePrefab;

		private const float SILO_HALVES_ROTATE_SPEED_IN_M_PER_S = 15;
		private const float SILO_TARGET_ANGLE_IN_DEGREES = 45;
		private static Vector3 NUKE_SPAWN_POSITION_ADJUSTMENT = new Vector3(0, -0.3f, 0);

        protected override PrioritisedSoundKey ConstructionCompletedSoundKey { get { return PrioritisedSoundKeys.Completed.Ultra; } }
        public override TargetValue TargetValue { get { return TargetValue.High; } }

        protected override void AddBuildRateBoostProviders(
            IGlobalBoostProviders globalBoostProviders,
            IList<IObservableCollection<IBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(_factoryProvider.GlobalBoostProviders.BuildingBuildRate.UltrasProviders);
        }

        protected override void OnStaticInitialised()
		{
            base.OnStaticInitialised();

            Helper.AssertIsNotNull(leftSiloHalf, rightSiloHalf, nukeMissilePrefab);

			leftSiloHalf.StaticInitialise();
			rightSiloHalf.StaticInitialise();

			_spinner = gameObject.GetComponentInChildren<NukeSpinner>();
			Assert.IsNotNull(_spinner);
			_spinner.StaticInitialise();

            _nukeStats = GetComponent<NukeProjectileStats>();
			Assert.IsNotNull(_nukeStats);
            AddAttackCapability(TargetType.Cruiser);
            AddDamageStats(new DamageCapability(_nukeStats.Damage, AttackCapabilities));
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
			_launchedNuke = Instantiate(nukeMissilePrefab);
			_launchedNuke.transform.position = transform.position + NUKE_SPAWN_POSITION_ADJUSTMENT;

			ITargetFilter targetFilter = _factoryProvider.TargetFactories.FilterFactory.CreateExactMatchTargetFilter(_enemyCruiser);
            _launchedNuke.Initialise(_nukeStats, targetFilter, _enemyCruiser, _factoryProvider, this);

            // Make nuke face upwards (rotation is set in Initialise() above)
            _launchedNuke.transform.eulerAngles = new Vector3(0, 0, 90);
		}

		private void SiloHalf_ReachedDesiredAngle(object sender, EventArgs e)
		{
			leftSiloHalf.ReachedDesiredAngle -= SiloHalf_ReachedDesiredAngle;
			_launchedNuke.Launch();
		}

        protected override List<Renderer> GetInGameRenderers()
        {
            List<Renderer> renderers = base.GetInGameRenderers();

            renderers.Add(_spinner.Renderer);
            renderers.Add(leftSiloHalf.Renderer);
            renderers.Add(rightSiloHalf.Renderer);

            return renderers;
        }
	}
}
