using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using System;
using System.Collections.Generic;

namespace BattleCruisers.UI.BattleScene
{
    public class PrefabOrganiser : IPrefabOrganiser
    {
        private readonly ILoadout _playerLoadout;
        private readonly IPrefabFactory _prefabFactory;
		private readonly IBuildingGroupFactory _buildingGroupFactory;

		// User needs to be able to build at least one building
		private const int MIN_NUM_OF_BUILDING_GROUPS = 1;
		// Currently only support 6 types of buildings, so the UI is optimsed for this.  Ie, there is no space for more!
		private const int MAX_NUM_OF_BUILDING_GROUPS = 6;

        public PrefabOrganiser(ILoadout playerLoadout, IPrefabFactory prefabFactory, IBuildingGroupFactory buildingGroupFactory)
        {
            Helper.AssertIsNotNull(playerLoadout, prefabFactory, buildingGroupFactory);

            _playerLoadout = playerLoadout;
            _prefabFactory = prefabFactory;
            _buildingGroupFactory = buildingGroupFactory;
        }

        public IList<IBuildingGroup> GetBuildingGroups()
        {
			IDictionary<BuildingCategory, IList<IBuildableWrapper<IBuilding>>> buildings = GetBuildingsFromKeys(_playerLoadout, _prefabFactory);
            return CreateBuildingGroups(buildings, _buildingGroupFactory);
        }

		private IDictionary<BuildingCategory, IList<IBuildableWrapper<IBuilding>>> GetBuildingsFromKeys(ILoadout loadout, IPrefabFactory prefabFactory)
		{
			IDictionary<BuildingCategory, IList<IBuildableWrapper<IBuilding>>> categoryToBuildings = new Dictionary<BuildingCategory, IList<IBuildableWrapper<IBuilding>>>();

            foreach (BuildingCategory category in Enum.GetValues(typeof(BuildingCategory)))
			{
				List<BuildingKey> buildingKeys = loadout.GetBuildingKeys(category);

				IList<IBuildableWrapper<IBuilding>> buildings = new List<IBuildableWrapper<IBuilding>>();
                categoryToBuildings.Add(category, buildings);

				foreach (BuildingKey buildingKey in buildingKeys)
				{
					IBuildableWrapper<IBuilding> buildingWrapper = prefabFactory.GetBuildingWrapperPrefab(buildingKey).UnityObject;
					categoryToBuildings[buildingWrapper.Buildable.Category].Add(buildingWrapper);
				}
			}

			return categoryToBuildings;
		}

		private IList<IBuildingGroup> CreateBuildingGroups(
            IDictionary<BuildingCategory, IList<IBuildableWrapper<IBuilding>>> buildingCategoryToGroups,
            IBuildingGroupFactory buildingGroupFactory)
		{
			IList<IBuildingGroup> buildingGroups = new List<IBuildingGroup>();

			foreach (KeyValuePair<BuildingCategory, IList<IBuildableWrapper<IBuilding>>> categoryToBuildings in buildingCategoryToGroups)
			{
				IBuildingGroup group = buildingGroupFactory.CreateBuildingGroup(categoryToBuildings.Key, categoryToBuildings.Value);
				buildingGroups.Add(group);
			}

			if (buildingGroups.Count < MIN_NUM_OF_BUILDING_GROUPS
				|| buildingGroups.Count > MAX_NUM_OF_BUILDING_GROUPS)
			{
				throw new InvalidProgramException();
			}

			return buildingGroups;
		}

		public IDictionary<UnitCategory, IList<IBuildableWrapper<IUnit>>> GetUnits()
		{
			IDictionary<UnitCategory, IList<IBuildableWrapper<IUnit>>> categoryToUnits = new Dictionary<UnitCategory, IList<IBuildableWrapper<IUnit>>>();

			foreach (UnitCategory unitCategory in Enum.GetValues(typeof(UnitCategory)))
			{
				List<UnitKey> unitKeys = _playerLoadout.GetUnitKeys(unitCategory);

				if (unitKeys.Count != 0)
				{
					categoryToUnits[unitCategory] = GetUnits(unitKeys, _prefabFactory);
				}
			}

			return categoryToUnits;
		}

		private IList<IBuildableWrapper<IUnit>> GetUnits(IList<UnitKey> unitKeys, IPrefabFactory prefabFactory)
		{
			IList<IBuildableWrapper<IUnit>> unitWrappers = new List<IBuildableWrapper<IUnit>>(unitKeys.Count);

			foreach (UnitKey unitKey in unitKeys)
			{
				IBuildableWrapper<IUnit> unitWrapper = prefabFactory.GetUnitWrapperPrefab(unitKey);
				unitWrappers.Add(unitWrapper);
			}

			return unitWrappers;
		}
    }
}
