using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.TargetFinders;
using BattleCruisers.TestScenes;
using BattleCruisers.Units.Aircraft;
using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BattleCruisers.TestScenes.Utilities;

namespace BattleCruisers.TestScenes.Aircraft
{
	public class AircraftBombingTestsGod : MonoBehaviour 
	{
		public BomberController bomberToLeft;
		public BomberController bomberToRight;

		public GameObject target;

		void Start() 
		{
			IFactionable factionObject = Substitute.For<IFactionable>();
			factionObject.GameObject.Returns(target);

			ITargetFinder targetFinder = Substitute.For<ITargetFinder>();
			targetFinder.FindTarget().Returns(factionObject);

			ITargetFinderFactory targetFinderFactory = Substitute.For<ITargetFinderFactory>();
			targetFinderFactory.BomberTargetFinder.Returns(targetFinder);

			Helper helper = new Helper();

			helper.InitialiseBuildable(bomberToRight, targetFinderFactory: targetFinderFactory);
			bomberToRight.StartConstruction();

			helper.InitialiseBuildable(bomberToLeft, targetFinderFactory: targetFinderFactory);
			bomberToLeft.StartConstruction();
		}
	}
}
