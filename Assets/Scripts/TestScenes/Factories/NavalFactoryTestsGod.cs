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
		public NavalFactory navalFactory;
		public AttackBoatController attackBoatPrefab;

		void Start () 
		{
			Helper helper = new Helper();

			IBuildableFactory buildableFactory = Substitute.For<IBuildableFactory>();
			buildableFactory.CreateUnit(attackBoatPrefab).Returns(Instantiate(attackBoatPrefab));

			helper.InitialiseBuildable(navalFactory, buildableFactory: buildableFactory, parentCruiserDirection: Direction.Right);

			navalFactory.CompletedBuildable += NavalFactory_CompletedBuildable;
			navalFactory.StartConstruction();
		}

		private void NavalFactory_CompletedBuildable(object sender, EventArgs e)
		{
			navalFactory.Unit = attackBoatPrefab;
		}
	}
}
