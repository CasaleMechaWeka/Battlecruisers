using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Fetchers;
using BattleCruisers.Fetchers.PrefabKeys;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.AI
{
	public class Bot
	{
		private Cruiser _friendlyCruiser, _enemyCruiser;
		private IList<BuildingKey> _buildOrder;
		private PrefabFactory _prefabFactory;
		private int _buildOrderIndex;

		public Bot(Cruiser friendlyCruiser, Cruiser enemyCruiser, IList<BuildingKey> buildOrder, PrefabFactory prefabFactory)
		{
			_friendlyCruiser = friendlyCruiser;
			_enemyCruiser = enemyCruiser;
			_buildOrder = buildOrder;
			_prefabFactory = prefabFactory;
			_buildOrderIndex = 0;
		}

		public void Start()
		{
			BuildNextBuilding();
		}

		private void BuildNextBuilding()
		{
			if (_buildOrderIndex < _buildOrder.Count)
			{
				BuildingKey buildingKey = _buildOrder[_buildOrderIndex++];

				Logging.Log(Tags.AI, "BuildNextBuilding: " + buildingKey.PrefabPath);

				BuildingWrapper buildingWrapperPrefab = _prefabFactory.GetBuildingWrapperPrefab(buildingKey);
				ISlot slot = _friendlyCruiser.GetFreeSlot(buildingWrapperPrefab.building.slotType);
				Assert.IsNotNull(slot);

				Building building = _friendlyCruiser.ConstructBuilding(buildingWrapperPrefab, slot);
				building.CompletedBuildable += Building_CompletedBuildable;
			}
		}

		private void Building_CompletedBuildable(object sender, EventArgs e)
		{
			Building building = sender as Building;
			Assert.IsNotNull(building);
			building.CompletedBuildable -= Building_CompletedBuildable;

			Logging.Log(Tags.AI, "Building_CompletedBuildable: " + building);

			BuildNextBuilding();
		}
	}
}
