using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.TestScenes.Utilities;
using NSubstitute;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.TestScenes.Factories
{
	public class NavalFactoryTestsGod : MonoBehaviour 
	{
		public NavalFactory navalFactoryFacingRight, navalFactoryFacingLeft;
		public AttackBoatController attackBoatPrefab;

		void Start () 
		{
			Helper helper = new Helper();

			IBuildableFactory buildableFactory = Substitute.For<IBuildableFactory>();
			buildableFactory.CreateUnit(attackBoatPrefab).Returns(callInfo => Instantiate(attackBoatPrefab));

			helper.InitialiseBuildable(navalFactoryFacingRight, buildableFactory: buildableFactory, parentCruiserDirection: Direction.Right);
			helper.InitialiseBuildable(navalFactoryFacingLeft, buildableFactory: buildableFactory, parentCruiserDirection: Direction.Left);

			navalFactoryFacingRight.CompletedBuildable += NavalFactory_CompletedBuildable;
			navalFactoryFacingLeft.CompletedBuildable += NavalFactory_CompletedBuildable;

			navalFactoryFacingRight.StartConstruction();
			navalFactoryFacingLeft.StartConstruction();
		}

		private void NavalFactory_CompletedBuildable(object sender, EventArgs e)
		{
			((NavalFactory)sender).Unit = attackBoatPrefab;
		}
	}
}
