using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Offensive
{
	public class DeathstarLauncherController : Building
	{
		private Unit _deathstar;

		public UnitWrapper deathstarPrefab;

		public override TargetValue TargetValue { get { return TargetValue.High; } }

		public override void StaticInitialise()
		{
			base.StaticInitialise();

			Assert.IsNotNull(deathstarPrefab);
		}

		protected override void OnInitialised()
		{
			base.OnInitialised();

			_deathstar = _factoryProvider.PrefabFactory.CreateUnit(deathstarPrefab);

			// FELIX  Set spawn position and rotation

			_deathstar.Initialise(_parentCruiser, _enemyCruiser, _uiManager, _factoryProvider);
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			_deathstar.StartConstruction();
		}

		protected override void OnDestroyed()
		{
			base.OnDestroyed();

			_deathstar.Destroy();
		}
	}
}
