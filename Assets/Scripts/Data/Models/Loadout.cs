using System;
using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils;
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
		private List<UnitKey> _units;

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