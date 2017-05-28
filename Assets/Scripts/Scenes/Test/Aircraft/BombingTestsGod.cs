using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Scenes.Test;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Units.Aircraft;
using BattleCruisers.Units.Aircraft.Providers;
using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BcUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Aircraft
{
	public class BombingTestsGod : MonoBehaviour 
	{
		public BomberController bomberToLeft, bomberToRight;
		public List<Vector2> patrolPoints;

		public GameObject target;

		void Start() 
		{
			Helper helper = new Helper();
			BcUtils.IFactoryProvider factoryProvider = helper.CreateFactoryProvider(target);
			IAircraftProvider aircraftProvider = helper.CreateAircraftProvider(bomberPatrolPoints: patrolPoints);

			helper.InitialiseBuildable(bomberToRight, factoryProvider: factoryProvider, aircraftProvider: aircraftProvider, parentCruiserDirection: Direction.Left);
			bomberToRight.StartConstruction();

			helper.InitialiseBuildable(bomberToLeft, factoryProvider: factoryProvider, aircraftProvider: aircraftProvider, parentCruiserDirection: Direction.Right);
			bomberToLeft.StartConstruction();
		}
	}
}
