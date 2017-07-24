using System;
using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.PrefabKeys;
using BattleCruisers.Fetchers;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.AI
{
    // FELIX  Delete?  Refactor?  Use new AI classes :)
    public class Bot
	{
		private Cruiser _friendlyCruiser;
//		private Cruiser _enemyCruiser;
		private IList<BuildingKey> _buildOrder;
		private IPrefabFactory _prefabFactory;
		private int _buildOrderIndex;

		public Bot(Cruiser friendlyCruiser, Cruiser enemyCruiser, IList<BuildingKey> buildOrder, IPrefabFactory prefabFactory)
		{
			_friendlyCruiser = friendlyCruiser;
//			_enemyCruiser = enemyCruiser;
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

				IBuildableWrapper<Building> buildingWrapperPrefab = _prefabFactory.GetBuildingWrapperPrefab(buildingKey);
                ISlot slot = _friendlyCruiser.GetFreeSlot(buildingWrapperPrefab.Buildable.slotType);
				Assert.IsNotNull(slot);

                // FELIX  Ugly!  Make cruiser use the type of UnityObject, to avoid cast!!
                //Building building = _friendlyCruiser.ConstructBuilding((BuildingWrapper)buildingWrapperPrefab.UnityObject, slot);
				//building.CompletedBuildable += Building_CompletedBuildable;
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
