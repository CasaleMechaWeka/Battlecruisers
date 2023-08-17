using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
using BattleCruisers.UI.ScreensScene.ShopScreen;
using BattleCruisers.Utils;
using UnityEditor;
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

        // Captain Logic

//        [SerializeField]
        private CaptainExoKey _currentCaptain;
        public CaptainExoKey CurrentCaptain
        {
            get => _currentCaptain;
            set => _currentCaptain = value;
        }

/*        private IList<int> _captainExos;
        public IList<int> CaptainExos
        {
            get => _captainExos;
            set => _captainExos = value;
        }

        private List<int> _heckles;
        public List<int> Heckles
        {
            get => _heckles;
            set => _heckles = value;
        }*/

        private List<int> _currentHeckles = new List<int>{ 0, 1, 2 };
        public List<int> CurrentHeckles
        {
            get => _currentHeckles;
            set => _currentHeckles = value;
        }

        [SerializeField]
        private List<string> _ownedExosKeys = new List<string>();
        public IReadOnlyList<string> OwnedExosKeys => _ownedExosKeys;



        public Loadout(
            HullKey hull,
            List<BuildingKey> buildings,
            List<UnitKey> units,
            Dictionary<BuildingCategory, List<BuildingKey>> buildLimt,
            Dictionary<UnitCategory, List<UnitKey>> unitLimit)
        {
            Hull = hull;
            _buildings = buildings;
            _units = units;
            _builds = buildLimt;
            _unit = unitLimit;
            _currentCaptain =  new CaptainExoKey("CaptainExo000");  // "CaptainExo000" is Charlie, the default captain        
        }

        public bool Is_buildsNull()
        {
            return _builds == null;
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
                {UnitCategory.Naval, ships },
                {UnitCategory.Aircraft, aircraft }
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
            }
        }

        public void RemoveUnit(UnitKey unitToRemove)
        {
            bool removedSuccessfully = _units.Remove(unitToRemove);
            Assert.IsTrue(removedSuccessfully);
        }

        //functions to handle the lists for the buildables
        public void AddbuildItem(BuildingCategory category, BuildingKey keyToAdd)
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
/*        public void AddCaptain(int key)
        {
            _captainExos.Add(key);
        }
        public void RemoveCaptain(int key)
        {
            _captainExos.Remove(key);
        }
        public void AddHeckle(int index)
        {
            _heckles.Add(index);
        }
        public void RemoveHeckle(int index)
        {
            _heckles.Remove(index);
        }*/
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
            if (!_ownedExosKeys.Contains(exoKey))
            {
                _ownedExosKeys.Add(exoKey);
            }
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
    }
}