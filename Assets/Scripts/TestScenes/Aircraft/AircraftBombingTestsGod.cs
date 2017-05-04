using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.TestScenes;
using BattleCruisers.Units.Aircraft;
using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BattleCruisers.TestScenes.Utilities;
using BattleCruisers.Targets;

namespace BattleCruisers.TestScenes.Aircraft
{
	public class AircraftBombingTestsGod : MonoBehaviour 
	{
		public BomberController bomberToLeft;
		public BomberController bomberToRight;

		public GameObject target;

		void Start() 
		{
			Helper helper = new Helper();
			ITargetsFactory targetsFactory = helper.CreateTargetsFactory(target);

			helper.InitialiseBuildable(bomberToRight, targetsFactory: targetsFactory);
			bomberToRight.StartConstruction();

			helper.InitialiseBuildable(bomberToLeft, targetsFactory: targetsFactory);
			bomberToLeft.StartConstruction();
		}
	}
}
