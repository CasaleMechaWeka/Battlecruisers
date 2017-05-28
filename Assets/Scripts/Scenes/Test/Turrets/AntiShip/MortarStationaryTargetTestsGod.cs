using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Targets;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Units.Aircraft;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BcUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Turrets.AntiShip
{
	public class MortarStationaryTargetTestsGod : MonoBehaviour 
	{
		private Helper _helper;

		public GameObject target;
		public DefensiveTurret mortarLeftLow, mortarLeftMiddle, mortarLeftHigh;
		public DefensiveTurret mortarRightLow, mortarRightMiddle, mortarRightHigh;

		void Start () 
		{
			_helper = new Helper();

			SetupPair(mortarLeftLow, target);
			SetupPair(mortarLeftMiddle, target);
			SetupPair(mortarLeftHigh, target);

			SetupPair(mortarRightLow, target);
			SetupPair(mortarRightMiddle, target);
			SetupPair(mortarRightHigh, target);
		}

		private void SetupPair(DefensiveTurret mortar, GameObject target)
		{
			BcUtils.IFactoryProvider factoryProvider = _helper.CreateFactoryProvider(target);
			_helper.InitialiseBuildable(mortar, factoryProvider: factoryProvider);
			mortar.StartConstruction();
		}
	}
}
