using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Data.Models
{
    [Serializable]
    public class Loadout : ILoadout
    {
        [SerializeField]
        private HullKey _hull;

        [SerializeField]
        private List<BuildingKey> _buildings;

        [SerializeField]
        private Dictionary<BuildingCategory, List<BuildingKey>> _builds;

        [SerializeField]
        private List<UnitKey> _units;

        [SerializeField]
        private Dictionary<UnitCategory, List<UnitKey>> _unit;

        public HullKey Hull
        {
            get { return _hull; }
            set
            {
                Assert.IsNotNull(value);
                _hull = value;
            }
        }

        [SerializeField]
        private CaptainExoKey _currentCaptain;
        public CaptainExoKey CurrentCaptain
        {
            get => _currentCaptain;
            set => _currentCaptain = value;
        }
        private List<int> _currentHeckles = UnlockedHeckles();
        public List<int> CurrentHeckles
        {
            get => _currentHeckles;
            set => _currentHeckles = value;
        }

        [SerializeField]
        private int _selectedBodykit;     // index of prefabs
        public int SelectedBodykit
        {
            get => _selectedBodykit;
            set => _selectedBodykit = value;
        }

        [SerializeField]
        private List<int> _selectedVariants;  // index of Prefabs
        public List<int> SelectedVariants
        {
            get => _selectedVariants;
            set => _selectedVariants = value;
        }

        [SerializeField]
        private List<string> _ownedExosKeys = new List<string>();
        public IReadOnlyList<string> OwnedExosKeys => _ownedExosKeys;

        public Loadout(
            HullKey hull,
            List<BuildingKey> buildings,
            List<UnitKey> units,
            Dictionary<BuildingCategory, List<BuildingKey>> buildLimt,
            Dictionary<UnitCategory, List<UnitKey>> unitLimit,
            GameModel gameModel = null)
        {
            Hull = hull;
            _buildings = buildings;
            _units = units;
            _builds = buildLimt;
            _unit = unitLimit;
            _currentCaptain = new CaptainExoKey("CaptainExo000");  // "CaptainExo000" is Charlie, the default captain
            _selectedBodykit = -1;
            _selectedVariants = new List<int>();
        }

        public bool Is_buildsNull()
        {
            return _builds == null;
        }

        public async Task<VariantPrefab> GetSelectedUnitVariant(IPrefabFactory prefabFactory, IUnit unit)
        {
            foreach (int index in _selectedVariants)
            {
                IPrefabKey variantKey = StaticPrefabKeys.Variants.GetVariantKey(index);
                VariantPrefab variantPrefab = await prefabFactory.GetVariant(variantKey);
                if (variantPrefab.IsUnit())
                {
                    if (unit.PrefabName.ToUpper().Replace("(CLONE)", "") == variantPrefab.GetPrefabKey().PrefabName.ToUpper())
                    {
                        return variantPrefab;
                    }
                }
            }
            return null;
        }

        public async Task<int> GetSelectedUnitVariantIndex(IPrefabFactory prefabFactory, IUnit unit)
        {
            foreach (int index in _selectedVariants)
            {
                IPrefabKey variantKey = StaticPrefabKeys.Variants.GetVariantKey(index);
                VariantPrefab variantPrefab = await prefabFactory.GetVariant(variantKey);
                if (variantPrefab.IsUnit())
                {
                    if (unit.PrefabName.ToUpper().Replace("(CLONE)", "") == variantPrefab.GetPrefabKey().PrefabName.ToUpper())
                    {
                        return index;
                    }
                }
            }
            return -1;
        }

        public async Task<int> GetSelectedUnitVariantIndex(IPvPPrefabFactory prefabFactory, IPvPUnit unit)
        {
            foreach (int index in _selectedVariants)
            {
                IPrefabKey variantKey = StaticPrefabKeys.Variants.GetVariantKey(index);
                VariantPrefab variantPrefab = await prefabFactory.GetVariant(variantKey);
                if (variantPrefab.IsUnit())
                {
                    if (unit.PrefabName.ToUpper().Replace("(CLONE)", "") == "PVP" + variantPrefab.GetPrefabKey().PrefabName.ToUpper())
                    {
                        return index;
                    }
                }
            }
            return -1;
        }

        public async Task<VariantPrefab> GetSelectedBuildingVariant(IPrefabFactory prefabFactory, IBuilding building)
        {
            foreach (int index in _selectedVariants)
            {
                IPrefabKey variantKey = StaticPrefabKeys.Variants.GetVariantKey(index);
                VariantPrefab variantPrefab = await prefabFactory.GetVariant(variantKey);
                if (!variantPrefab.IsUnit())
                {
                    if (building.PrefabName.ToUpper().Replace("(CLONE)", "") == variantPrefab.GetPrefabKey().PrefabName.ToUpper())
                    {
                        return variantPrefab;
                    }
                }
            }
            return null;
        }

        public async Task<int> GetSelectedBuildingVariantIndex(IPrefabFactory prefabFactory, IBuilding building)
        {
            foreach (int index in _selectedVariants)
            {
                IPrefabKey variantKey = StaticPrefabKeys.Variants.GetVariantKey(index);
                VariantPrefab variantPrefab = await prefabFactory.GetVariant(variantKey);
                if (!variantPrefab.IsUnit())
                {
                    if (building.PrefabName.ToUpper().Replace("(CLONE)", "") == variantPrefab.GetPrefabKey().PrefabName.ToUpper())
                    {
                        return index;
                    }
                }
            }
            return -1;
        }

        public async Task<int> GetSelectedBuildingVariantIndex(IPvPPrefabFactory prefabFactory, IPvPBuilding building)
        {
            foreach (int index in _selectedVariants)
            {
                IPrefabKey variantKey = StaticPrefabKeys.Variants.GetVariantKey(index);
                VariantPrefab variantPrefab = await prefabFactory.GetVariant(variantKey);
                if (!variantPrefab.IsUnit())
                {
                    if (building.PrefabName.ToUpper().Replace("(CLONE)", "") == "PVP" + variantPrefab.GetPrefabKey().PrefabName.ToUpper())
                    {
                        return index;
                    }
                }
            }
            return -1;
        }

        public void RemoveCurrentSelectedVariant(int index)
        {
            _selectedVariants.Remove(index);
        }
        public void AddSelectedVariant(int index)
        {
            _selectedVariants.Add(index);
        }

        public void Create_buildsAnd_units()
        {
            List<BuildingKey> limit = _buildings;
            List<BuildingKey> factories = new List<BuildingKey>();
            List<BuildingKey> defence = new List<BuildingKey>();
            List<BuildingKey> offense = new List<BuildingKey>();
            List<BuildingKey> tactical = new List<BuildingKey>();
            List<BuildingKey> Ultra = new List<BuildingKey>();
            foreach (BuildingKey key in limit)
            {
                switch (key.BuildingCategory)
                {
                    case BuildingCategory.Factory:
                        factories.Add(key);
                        break;
                    case BuildingCategory.Defence:
                        defence.Add(key);
                        break;
                    case BuildingCategory.Offence:
                        offense.Add(key);
                        break;
                    case BuildingCategory.Tactical:
                        tactical.Add(key);
                        break;
                    case BuildingCategory.Ultra:
                        Ultra.Add(key);
                        break;
                    default:
                        break;
                }
            }
            Dictionary<BuildingCategory, List<BuildingKey>> buildables = new()
            {
                { BuildingCategory.Factory, factories },
                { BuildingCategory.Defence, defence },
                { BuildingCategory.Offence, offense },
                { BuildingCategory.Tactical, tactical },
                { BuildingCategory.Ultra, Ultra }
            };
            _builds = buildables;

            List<UnitKey> units = _units;
            List<UnitKey> ships = new();
            List<UnitKey> aircraft = new();
            foreach (UnitKey unit in units)
            {
                if (unit.UnitCategory == UnitCategory.Naval)
                    ships.Add(unit);
                else if (unit.UnitCategory == UnitCategory.Aircraft)
                    aircraft.Add(unit);
                else
                    break;
            }
            Dictionary<UnitCategory, List<UnitKey>> unitlimit = new()
            {
                { UnitCategory.Naval, ships },
                { UnitCategory.Aircraft, aircraft }
            };
            _unit = unitlimit;
        }
        public IList<BuildingKey> GetBuildings(BuildingCategory buildingCategory)
        {
            return _buildings.Where(buildingKey => buildingKey.BuildingCategory == buildingCategory).ToList();
        }

        public IList<UnitKey> GetUnits(UnitCategory unitCategory)
        {
            return _units.Where(unitKey => unitKey.UnitCategory == unitCategory).ToList();
        }

        public void AddBuilding(BuildingKey buildingToAdd)
        {
            if (!_buildings.Contains(buildingToAdd))
            {
                _buildings.Add(buildingToAdd);

                if (GetBuildingListSize(buildingToAdd.BuildingCategory) < 5)
                    AddBuildingItem(buildingToAdd.BuildingCategory, buildingToAdd);
            }
        }

        public void RemoveBuilding(BuildingKey buildingToRemove)
        {
            bool removedSuccessfully = _buildings.Remove(buildingToRemove);
            Assert.IsTrue(removedSuccessfully);
        }

        public void AddUnit(UnitKey unitToAdd)
        {
            if (!_units.Contains(unitToAdd))
            {
                _units.Add(unitToAdd);

                if (GetUnitListSize(unitToAdd.UnitCategory) < 5)
                    AddUnitItem(unitToAdd.UnitCategory, unitToAdd);
            }
        }

        public void RemoveUnit(UnitKey unitToRemove)
        {
            bool removedSuccessfully = _units.Remove(unitToRemove);
            Assert.IsTrue(removedSuccessfully);
        }

        //functions to handle the lists for the buildables
        public void AddBuildingItem(BuildingCategory category, BuildingKey keyToAdd)
        {
            List<BuildingKey> builds = _builds[category];
            builds.Add(keyToAdd);
            _builds[category] = builds;
        }

        public void AddUnitItem(UnitCategory category, UnitKey keyToAdd)
        {
            List<UnitKey> unitList = _unit[category];
            unitList.Add(keyToAdd);
            _unit[category] = unitList;
        }

        private static List<int> UnlockedHeckles()
        {
            List<int> unlockedHeckles = new List<int>();
            int numHecklesUnlocked = 3;
            while (unlockedHeckles.Count < numHecklesUnlocked)
            {
                int unlockHeckle = UnityEngine.Random.Range(0, 279);
                if (!unlockedHeckles.Contains(unlockHeckle))
                {
                    unlockedHeckles.Add(unlockHeckle);
                }
            }
            return unlockedHeckles;
        }

        public void RemoveBuildItem(BuildingCategory category, BuildingKey keyToRemove)
        {
            List<BuildingKey> builds = _builds[category];
            bool removedSuccessfully = builds.Remove(keyToRemove);
            Assert.IsTrue(removedSuccessfully);
            _builds[category] = builds;
        }

        public void RemoveUnitItem(UnitCategory category, UnitKey keyToRemove)
        {
            List<UnitKey> unitList = _unit[category];
            bool removedSuccessfully = unitList.Remove(keyToRemove);
            Assert.IsTrue(removedSuccessfully);
            _unit[category] = unitList;
        }

        public List<BuildingKey> GetBuildingKeys(BuildingCategory buildingCategory)
        {
            List<BuildingKey> builds = _builds[buildingCategory].ToList();
            Assert.IsNotNull(builds);
            return builds;
        }
        public List<UnitKey> GetUnitKeys(UnitCategory unitCategory)
        {
            List<UnitKey> unitList = _unit[unitCategory];
            return unitList;
        }

        public int GetBuildingListSize(BuildingCategory category)
        {
            List<BuildingKey> builds = _builds[category];
            return builds.Count;
        }

        public int GetUnitListSize(UnitCategory category)
        {
            List<UnitKey> unitList = _unit[category];
            return unitList.Count;
        }

        public bool IsBuildingInList(BuildingCategory category, BuildingKey key)
        {
            List<BuildingKey> buildingKeys = _builds[category];
            if (buildingKeys.Contains(key))
                return true;
            return false;
        }

        public bool IsUnitInList(UnitCategory category, UnitKey key)
        {
            List<UnitKey> unitKeys = _unit[category];
            if (unitKeys.Contains(key))
                return true;
            return false;
        }

        public bool OwnsExo(string exoKey)
        {
            return _ownedExosKeys.Contains(exoKey);
        }

        public void PurchaseExo(string exoKey)
        {
            if (_ownedExosKeys != null)
            {
                if (!_ownedExosKeys.Contains(exoKey))
                {
                    _ownedExosKeys.Add(exoKey);
                }
            }
            else
            {
                _ownedExosKeys = new List<string>();
                _ownedExosKeys.Add(exoKey);
            }
        }

        public Dictionary<BuildingCategory, List<BuildingKey>> GetBuildLimits()
        {
            return _builds;
        }

        public Dictionary<UnitCategory, List<UnitKey>> GetUnitLimits()
        {
            return _unit;
        }

        public override bool Equals(object obj)
        {
            Loadout other = obj as Loadout;

            return other != null
                && Hull.SmartEquals(other.Hull)
                && Enumerable.SequenceEqual(_buildings, other._buildings)
                && Enumerable.SequenceEqual(_units, other._units);
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(_hull, _buildings, _units);
        }

        /*        public string GetSelectedVariantsAsString()
                {
                    string ret = string.Empty;
                    foreach(int i in _selectedVariants)
                    {
                        ret += i.ToString() + " ";
                    }
                    return ret;
                }*/
    }
}