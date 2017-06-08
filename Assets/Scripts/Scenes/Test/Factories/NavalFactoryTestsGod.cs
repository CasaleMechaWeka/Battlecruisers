using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Fetchers;
using BattleCruisers.Scenes.Test.Utilities;
using NSubstitute;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Factories
{
	public class NavalFactoryTestsGod : MonoBehaviour 
	{
		public NavalFactory navalFactoryFacingRight, navalFactoryFacingLeft;
		public UnitWrapper attackBoatPrefab;

		void Start () 
		{
			attackBoatPrefab.Initialise();

			Helper helper = new Helper();

			IPrefabFactory prefabFactory = Substitute.For<IPrefabFactory>();
			prefabFactory.CreateUnit(attackBoatPrefab).Returns(callInfo => 
			{
				UnitWrapper unitWrapper = Instantiate(attackBoatPrefab);
				unitWrapper.Initialise();
				return unitWrapper.Unit;
			});

			helper.InitialiseBuildable(navalFactoryFacingRight, prefabFactory: prefabFactory, parentCruiserDirection: Direction.Right);
			helper.InitialiseBuildable(navalFactoryFacingLeft, prefabFactory: prefabFactory, parentCruiserDirection: Direction.Left);

			navalFactoryFacingRight.CompletedBuildable += Factory_CompletedBuildable;
			navalFactoryFacingLeft.CompletedBuildable += Factory_CompletedBuildable;

			navalFactoryFacingRight.StartConstruction();
			navalFactoryFacingLeft.StartConstruction();
		}

		private void Factory_CompletedBuildable(object sender, EventArgs e)
		{
			((Factory)sender).UnitWrapper = attackBoatPrefab;
		}
	}
}
