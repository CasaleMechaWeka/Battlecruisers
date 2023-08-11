using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using System;
using System.Collections.Generic;
using UnityEngine;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using System.Threading.Tasks;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene
{
    public class PvPPrefabOrganiser : IPvPPrefabOrganiser
    {
        private readonly ILoadout _playerLoadout;
        private readonly IPvPPrefabFactory _prefabFactory;
        private readonly IPvPBuildingGroupFactory _buildingGroupFactory;

        // User needs to be able to build at least one building
        private const int MIN_NUM_OF_BUILDING_GROUPS = 1;
        // Currently only support 6 types of buildings, so the UI is optimsed for this.  Ie, there is no space for more!
        private const int MAX_NUM_OF_BUILDING_GROUPS = 6;

        public PvPPrefabOrganiser(ILoadout playerLoadout, IPvPPrefabFactory prefabFactory, IPvPBuildingGroupFactory buildingGroupFactory)
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

        private PvPBuildingKey ConvertToPvP(BuildingKey bKey)
        {
            switch (bKey.BuildingCategory)
            {
                case BuildingCategory.Defence:
                    return new PvPBuildingKey(PvPBuildingCategory.Defence, "PvP" + bKey.PrefabName);
                case BuildingCategory.Factory:
                    return new PvPBuildingKey(PvPBuildingCategory.Factory, "PvP" + bKey.PrefabName);
                case BuildingCategory.Offence:
                    return new PvPBuildingKey(PvPBuildingCategory.Offence, "PvP" + bKey.PrefabName);
                case BuildingCategory.Tactical:
                    return new PvPBuildingKey(PvPBuildingCategory.Tactical, "PvP" + bKey.PrefabName);
                case BuildingCategory.Ultra:
                    return new PvPBuildingKey(PvPBuildingCategory.Ultra, "PvP" + bKey.PrefabName);
                default:
                    throw new NullReferenceException();
            }
        }

        private PvPBuildingCategory convertToPvP(BuildingCategory category)
        {
            switch (category)
            {
                case BuildingCategory.Defence:
                    return PvPBuildingCategory.Defence;
                case BuildingCategory.Factory:
                    return PvPBuildingCategory.Factory;
                case BuildingCategory.Offence:
                    return PvPBuildingCategory.Offence;
                case BuildingCategory.Tactical:
                    return PvPBuildingCategory.Tactical;
                case BuildingCategory.Ultra:
                    return PvPBuildingCategory.Ultra;
                default:
                    throw new NullReferenceException();
            }
        }

        private IDictionary<PvPBuildingCategory, IList<IPvPBuildableWrapper<IPvPBuilding>>> GetBuildingsFromKeys(ILoadout loadout, IPvPPrefabFactory prefabFactory)
        {
            IDictionary<PvPBuildingCategory, IList<IPvPBuildableWrapper<IPvPBuilding>>> categoryToBuildings = new Dictionary<PvPBuildingCategory, IList<IPvPBuildableWrapper<IPvPBuilding>>>();

            foreach (BuildingCategory category in Enum.GetValues(typeof(BuildingCategory)))
            {
                //   IList<BuildingKey> buildingKeys = loadout.GetBuildings(category);
                IList<BuildingKey> buildingKeys = loadout.GetBuildingKeys(category);
                IList<PvPBuildingKey> pvp_buildingKeys = new List<PvPBuildingKey>();
                foreach (BuildingKey bKey in buildingKeys)
                {
                    pvp_buildingKeys.Add(ConvertToPvP(bKey));
                }
                IList<IPvPBuildableWrapper<IPvPBuilding>> buildings = new List<IPvPBuildableWrapper<IPvPBuilding>>();
                categoryToBuildings.Add(convertToPvP(category), buildings);

                foreach (PvPBuildingKey buildingKey in pvp_buildingKeys)
                {
                    IPvPBuildableWrapper<IPvPBuilding> buildingWrapper = prefabFactory.GetBuildingWrapperPrefab(buildingKey).UnityObject;
                    SynchedServerData.Instance.TryPreLoadBuildablePrefab(buildingWrapper.Buildable.Category, buildingWrapper.Buildable.PrefabName);
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


        private PvPUnitKey convertToPvP(UnitKey uKey)
        {
            switch (uKey.UnitCategory)
            {
                case UnitCategory.Aircraft:
                    return new PvPUnitKey(PvPUnitCategory.Aircraft, "PvP" + uKey.PrefabName);
                case UnitCategory.Naval:
                    return new PvPUnitKey(PvPUnitCategory.Naval, "PvP" + uKey.PrefabName);
                // case UnitCategory.Untouchable:
                //     return new PvPUnitKey(PvPUnitCategory.Untouchable, "PvP" + uKey.PrefabName);
                default:
                    throw new NullReferenceException();
            }
        }

        private PvPUnitCategory convertToPvP(UnitCategory category)
        {
            switch (category)
            {
                case UnitCategory.Aircraft:
                    return PvPUnitCategory.Aircraft;
                case UnitCategory.Naval:
                    return PvPUnitCategory.Naval;
                // case UnitCategory.Untouchable:
                //     return PvPUnitCategory.Untouchable;
                default:
                    throw new NullReferenceException();
            }
        }

        public IDictionary<PvPUnitCategory, IList<IPvPBuildableWrapper<IPvPUnit>>> GetUnits()
        {
            IDictionary<PvPUnitCategory, IList<IPvPBuildableWrapper<IPvPUnit>>> categoryToUnits = new Dictionary<PvPUnitCategory, IList<IPvPBuildableWrapper<IPvPUnit>>>();

            foreach (UnitCategory unitCategory in Enum.GetValues(typeof(UnitCategory)))
            {
                //   IList<UnitKey> unitKeys = _playerLoadout.GetUnits(unitCategory);
                IList<UnitKey> unitKeys = _playerLoadout.GetUnitKeys(unitCategory);
                IList<PvPUnitKey> pvp_unitKeys = new List<PvPUnitKey>();
                foreach (UnitKey uKey in unitKeys)
                {
                    PvPUnitKey _uKey = convertToPvP(uKey);
                    pvp_unitKeys.Add(_uKey);
                }

                if (pvp_unitKeys.Count != 0)
                {
                    categoryToUnits[convertToPvP(unitCategory)] = GetUnits(pvp_unitKeys, _prefabFactory);
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
