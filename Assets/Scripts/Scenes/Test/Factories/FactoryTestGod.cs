using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Fetchers;
using BattleCruisers.Targets;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Buildables.Units.Aircraft;
using NSubstitute;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Factories
{
	public class FactoryTestGod : MonoBehaviour 
	{
		public Factory factoryFacingRight, factoryFacingLeft;
		public UnitWrapper unitPrefab;

		void Start () 
		{
			unitPrefab.Initialise();

			Helper helper = new Helper();

			IPrefabFactory prefabFactory = Substitute.For<IPrefabFactory>();
			prefabFactory.CreateUnit(unitPrefab).Returns(callInfo => 
			{
				UnitWrapper unitWraper = Instantiate(unitPrefab);
				unitWraper.Initialise();
				return unitWraper.Unit;
			});

			helper.InitialiseBuildable(factoryFacingRight, prefabFactory: prefabFactory, parentCruiserDirection: Direction.Right);
			helper.InitialiseBuildable(factoryFacingLeft, prefabFactory: prefabFactory, parentCruiserDirection: Direction.Left);

			factoryFacingRight.CompletedBuildable += Factory_CompletedBuildable;
			factoryFacingRight.StartedBuildingUnit += Factory_StartedBuildingUnit;

			factoryFacingLeft.CompletedBuildable += Factory_CompletedBuildable;
			factoryFacingLeft.StartedBuildingUnit += Factory_StartedBuildingUnit;

			factoryFacingRight.StartConstruction();
			factoryFacingLeft.StartConstruction();
		}

		private void Factory_CompletedBuildable(object sender, EventArgs e)
		{
			((Factory)sender).UnitWrapper = unitPrefab;
		}

		protected virtual void Factory_StartedBuildingUnit(object sender, StartedConstructionEventArgs e) {	}
	}
}
