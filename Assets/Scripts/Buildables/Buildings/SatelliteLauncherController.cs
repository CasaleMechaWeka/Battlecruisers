using BattleCruisers.Buildables.Pools;
using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.BattleScene.ProgressBars;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings
{
	public abstract class SatelliteLauncherController : Building
	{
        private IUnit _satellite;

        public UnitWrapper satellitePrefab;

        protected abstract Vector3 SpawnPositionAdjustment { get; }

        public override void StaticInitialise(GameObject parent, HealthBarController healthBar)
		{
            base.StaticInitialise(parent, healthBar);
			Assert.IsNotNull(satellitePrefab);
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			_satellite = _factoryProvider.PrefabFactory.CreateUnit(satellitePrefab, _uiManager, _factoryProvider);
			_satellite.Position = transform.position + SpawnPositionAdjustment;

            _satellite.Activate(
                new BuildableActivationArgs(
                    ParentCruiser,
                    _enemyCruiser,
                    _cruiserSpecificFactories));

            _satellite.StartConstruction();
		}

		protected override void OnDestroyed()
		{
			base.OnDestroyed();

            if (BuildableState == BuildableState.Completed)
            {
                _satellite.Destroy();
			}
		}
	}
}
