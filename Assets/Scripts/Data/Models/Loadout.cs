using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
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

		private Dictionary<ItemType, List<BuildingKey>> _builds = new();

		
        [SerializeField]
        private List<BuildingKey> _factories;

        [SerializeField]
        private List<BuildingKey> _defence;

        [SerializeField]
        private List<BuildingKey> _offensive;

        [SerializeField]
        private List<BuildingKey> _tactical;

        [SerializeField]
        private List<BuildingKey> _ultra;

		[SerializeField]
		private List<UnitKey> _units;

		private Dictionary<ItemType, List<UnitKey>> _unit = new();

        public HullKey Hull
		{
			get { return _hull; }
			set 
			{ 
				Assert.IsNotNull(value);
				_hull = value; 
			}
		}

		public Loadout(
			HullKey hull,
			List<BuildingKey> buildings,
			List<UnitKey> units)
		{
			Hull = hull;
			_buildings = buildings;
			_units = units;
			AddList();
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
		public void AddbuildItem(ItemType category, BuildingKey keyToAdd)
		{
			List<BuildingKey> builds = _builds[category];
			builds.Add(keyToAdd);
			_builds[category] = builds;
		}

        public void AddUnitItem(ItemType category, UnitKey keyToAdd)
        {
            List<UnitKey> unitList = _unit[category];
            unitList.Add(keyToAdd);
            _unit[category] = unitList;
        }

		public void RemoveBuildItem(ItemType category, BuildingKey keyToRemove)
		{
            List<BuildingKey> builds = _builds[category];
            bool removedSuccessfully = builds.Remove(keyToRemove);
            Assert.IsTrue(removedSuccessfully);
            _builds[category] = builds;
        }

        public void RemoveUnitItem(ItemType category, UnitKey keyToRemove)
        {
            List<UnitKey> unitList = _unit[category];
            bool removedSuccessfully = unitList.Remove(keyToRemove);
            Assert.IsTrue(removedSuccessfully);
            _unit[category] = unitList;
        }

		public List<BuildingKey> GetBuildingKeys(ItemType buildingCategory)
		{
            List<BuildingKey> builds = _builds[buildingCategory].ToList();
			Assert.IsNotNull(builds);
			return builds;
        }

        public List<UnitKey> GetUnitKeys(ItemType unitCategory)
        {
            List<UnitKey> unitList = _unit[unitCategory];
            return unitList;
        }

		public int GetBuildingListSize(ItemType category)
		{
            List<BuildingKey> builds = _builds[category];
            return builds.Count;
		}

        public int GetUnitListSize(ItemType category)
		{
            List<UnitKey> unitList = _unit[category];
			return unitList.Count;
        }

		public void AddList()
		{
			List<BuildingKey> buildlist = new List<BuildingKey>();
			_builds.Add(ItemType.Factory, buildlist);
            _builds.Add(ItemType.Defense, buildlist);
            _builds.Add(ItemType.Offensive, buildlist);
            _builds.Add(ItemType.Tactical, buildlist);
            _builds.Add(ItemType.Ultra, buildlist);

			List<UnitKey> unitKeys = new List<UnitKey>();
			_unit.Add(ItemType.Ship, unitKeys);
			_unit.Add(ItemType.Aircraft, unitKeys);	
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