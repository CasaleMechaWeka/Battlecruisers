using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Data.Static;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Ultra;
        public override TargetValue TargetValue => TargetValue.High;

        protected override void AddBuildRateBoostProviders(
            IGlobalBoostProviders globalBoostProviders,
            IList<ObservableCollection<IBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(_cruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.UltrasProviders);
        }

        public override void StaticInitialise(HealthBarController healthBar)
		{
            base.StaticInitialise(healthBar);

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

        public override void Initialise(IUIManager uiManager, IFactoryProvider factoryProvider)
        {
            base.Initialise(uiManager, factoryProvider);

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

			ITargetFilter targetFilter = _factoryProvider.Targets.FilterFactory.CreateExactMatchTargetFilter(_enemyCruiser);
            _launchedNuke.Initialise(_factoryProvider);
            _launchedNuke.Activate(
                new TargetProviderActivationArgs<INukeStats>(
                    transform.position + NUKE_SPAWN_POSITION_ADJUSTMENT,
                    _nukeStats,
                    Vector2.zero,
                    targetFilter,
                    this,
                    _enemyCruiser));

            // Make nuke face upwards (rotation is set in Initialise() above)
            _launchedNuke.transform.eulerAngles = new Vector3(0, 0, 90);
		}

		private void SiloHalf_ReachedDesiredAngle(object sender, EventArgs e)
		{
			leftSiloHalf.ReachedDesiredAngle -= SiloHalf_ReachedDesiredAngle;
			_launchedNuke.Launch();
		}

        protected override List<SpriteRenderer> GetInGameRenderers()
        {
            return new List<SpriteRenderer>()
            {
                transform.FindNamedComponent<SpriteRenderer>("Base"),
                _spinner.Renderer,
                leftSiloHalf.Renderer,
                rightSiloHalf.Renderer
            };
        }
	}
}
