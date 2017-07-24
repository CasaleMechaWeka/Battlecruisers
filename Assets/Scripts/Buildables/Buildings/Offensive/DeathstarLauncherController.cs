using BattleCruisers.Buildables.Units;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Offensive
{
    public class DeathstarLauncherController : Building
	{
		private IUnit _deathstar;

		public UnitWrapper deathstarPrefab;

		private static Vector3 DEATHSTAR_SPAWN_POSITION_ADJUSTMENT = new Vector3(0, 0.015f, 0);

		public override TargetValue TargetValue { get { return TargetValue.High; } }

		public override void StaticInitialise()
		{
			base.StaticInitialise();

			Assert.IsNotNull(deathstarPrefab);
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			_deathstar = _factoryProvider.PrefabFactory.CreateUnit(deathstarPrefab);
			_deathstar.Position = transform.position + DEATHSTAR_SPAWN_POSITION_ADJUSTMENT;
			_deathstar.Initialise(_parentCruiser, _enemyCruiser, _uiManager, _factoryProvider);
			_deathstar.StartConstruction();
		}

		protected override void OnDestroyed()
		{
			base.OnDestroyed();

			_deathstar.Destroy();
		}
	}
}
