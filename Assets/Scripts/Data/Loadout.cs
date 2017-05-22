using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Fetchers;
using BattleCruisers.Data.PrefabKeys;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.Data
{
	[Serializable]
	public class Loadout
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
			private set { _hull = value; }
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

		public override bool Equals(object obj)
		{
			Loadout other = obj as Loadout;
			return other != null
				&& object.ReferenceEquals(Hull, other.Hull)
					|| (Hull != null && Hull.Equals(other.Hull))
				&& Enumerable.SequenceEqual(_buildings, other._buildings)
				&& Enumerable.SequenceEqual(_units, other._units);
		}

		public override int GetHashCode()
		{
			return this.GetHashCode(_hull, _buildings, _units);
		}
	}
}