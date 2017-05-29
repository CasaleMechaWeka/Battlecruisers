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

namespace BattleCruisers.Scenes.Test.Naval
{
	public class AttackBoatTestsGod : MonoBehaviour
	{
		public NavalFactory navalFactoryFacingRight, navalFactoryFacingLeft;
		public UnitWrapper attackBoatPrefab;

		void Start () 
		{
			Helper helper = new Helper();

			IPrefabFactory prefabFactory = Substitute.For<IPrefabFactory>();
			prefabFactory.CreateUnit(attackBoatPrefab).Returns(callInfo => Instantiate(attackBoatPrefab).Unit);

			helper.InitialiseBuildable(navalFactoryFacingRight, Faction.Reds, prefabFactory: prefabFactory, parentCruiserDirection: Direction.Right);
			helper.InitialiseBuildable(navalFactoryFacingLeft, Faction.Blues, prefabFactory: prefabFactory, parentCruiserDirection: Direction.Left);

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
