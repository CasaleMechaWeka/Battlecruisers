using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using System;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene
{
    public class PvPPrefabOrganiser : IPvPPrefabOrganiser
    {
        private readonly IPvPLoadout _playerLoadout;
        private readonly IPvPPrefabFactory _prefabFactory;
        private readonly IPvPBuildingGroupFactory _buildingGroupFactory;

        // User needs to be able to build at least one building
        private const int MIN_NUM_OF_BUILDING_GROUPS = 1;
        // Currently only support 6 types of buildings, so the UI is optimsed for this.  Ie, there is no space for more!
        private const int MAX_NUM_OF_BUILDING_GROUPS = 6;

        public PvPPrefabOrganiser(IPvPLoadout playerLoadout, IPvPPrefabFactory prefabFactory, IPvPBuildingGroupFactory buildingGroupFactory)
        {
            PvPHelper.AssertIsNotNull(playerLoadout, prefabFactory, buildingGroupFactory);

            _playerLoadout = playerLoadout;
            _prefabFactory = prefabFactory;
            _buildingGroupFactory = buildingGroupFactory;
        }

        public IList<IPvPBuildingGroup> GetBuildingGroups()
        {
            IDictionary<PvPBuildingCategory, IList<IPvPBuildableWrapper<IPvPBuilding>>> buildings = GetBuildingsFromKeys(_playerLoadout, _prefabFactory);
            return CreateBuildingGroups(buildings, _buildingGroupFactory);
        }

        private IDictionary<PvPBuildingCategory, IList<IPvPBuildableWrapper<IPvPBuilding>>> GetBuildingsFromKeys(IPvPLoadout loadout, IPvPPrefabFactory prefabFactory)
        {
            IDictionary<PvPBuildingCategory, IList<IPvPBuildableWrapper<IPvPBuilding>>> categoryToBuildings = new Dictionary<PvPBuildingCategory, IList<IPvPBuildableWrapper<IPvPBuilding>>>();

            foreach (PvPBuildingCategory category in Enum.GetValues(typeof(PvPBuildingCategory)))
            {
                IList<PvPBuildingKey> buildingKeys = loadout.GetBuildings(category);

                IList<IPvPBuildableWrapper<IPvPBuilding>> buildings = new List<IPvPBuildableWrapper<IPvPBuilding>>();
                categoryToBuildings.Add(category, buildings);

                foreach (PvPBuildingKey buildingKey in buildingKeys)
                {
                    IPvPBuildableWrapper<IPvPBuilding> buildingWrapper = prefabFactory.GetBuildingWrapperPrefab(buildingKey).UnityObject;
                    categoryToBuildings[buildingWrapper.Buildable.Category].Add(buildingWrapper);
                }
            }

            return categoryToBuildings;
        }

        private IList<IPvPBuildingGroup> CreateBuildingGroups(
            IDictionary<PvPBuildingCategory, IList<IPvPBuildableWrapper<IPvPBuilding>>> buildingCategoryToGroups,
            IPvPBuildingGroupFactory buildingGroupFactory)
        {
            IList<IPvPBuildingGroup> buildingGroups = new List<IPvPBuildingGroup>();

            foreach (KeyValuePair<PvPBuildingCategory, IList<IPvPBuildableWrapper<IPvPBuilding>>> categoryToBuildings in buildingCategoryToGroups)
            {
                IPvPBuildingGroup group = buildingGroupFactory.CreateBuildingGroup(categoryToBuildings.Key, categoryToBuildings.Value);
                buildingGroups.Add(group);
            }

            if (buildingGroups.Count < MIN_NUM_OF_BUILDING_GROUPS
                || buildingGroups.Count > MAX_NUM_OF_BUILDING_GROUPS)
            {
                throw new InvalidProgramException();
            }

            return buildingGroups;
        }

        public IDictionary<PvPUnitCategory, IList<IPvPBuildableWrapper<IPvPUnit>>> GetUnits()
        {
            IDictionary<PvPUnitCategory, IList<IPvPBuildableWrapper<IPvPUnit>>> categoryToUnits = new Dictionary<PvPUnitCategory, IList<IPvPBuildableWrapper<IPvPUnit>>>();

            foreach (PvPUnitCategory unitCategory in Enum.GetValues(typeof(PvPUnitCategory)))
            {
                IList<PvPUnitKey> unitKeys = _playerLoadout.GetUnits(unitCategory);

                if (unitKeys.Count != 0)
                {
                    categoryToUnits[unitCategory] = GetUnits(unitKeys, _prefabFactory);
                }
            }

            return categoryToUnits;
        }

        private IList<IPvPBuildableWrapper<IPvPUnit>> GetUnits(IList<PvPUnitKey> unitKeys, IPvPPrefabFactory prefabFactory)
        {
            IList<IPvPBuildableWrapper<IPvPUnit>> unitWrappers = new List<IPvPBuildableWrapper<IPvPUnit>>(unitKeys.Count);

            foreach (PvPUnitKey unitKey in unitKeys)
            {
                IPvPBuildableWrapper<IPvPUnit> unitWrapper = prefabFactory.GetUnitWrapperPrefab(unitKey);
                unitWrappers.Add(unitWrapper);
            }

            return unitWrappers;
        }
    }
}
