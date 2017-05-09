using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.TestScenes;
using BattleCruisers.TestScenes.Utilities;
using BattleCruisers.Units.Aircraft;
using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.TestScenes.Aircraft
{
	public class BombingTestsGod : MonoBehaviour 
	{
		public BomberController bomberToLeft;
		public BomberController bomberToRight;

		public GameObject target;

		void Start() 
		{
			Helper helper = new Helper();
			ITargetsFactory targetsFactory = helper.CreateTargetsFactory(target);

			helper.InitialiseBuildable(bomberToRight, targetsFactory: targetsFactory, parentCruiserDirection: Direction.Left);
			bomberToRight.StartConstruction();

			helper.InitialiseBuildable(bomberToLeft, targetsFactory: targetsFactory, parentCruiserDirection: Direction.Right);
			bomberToLeft.StartConstruction();
		}
	}
}
