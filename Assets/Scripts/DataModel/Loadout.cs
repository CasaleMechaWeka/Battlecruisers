using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Fetchers;
using BattleCruisers.Fetchers.PrefabKeys;
using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.DataModel
{
	public class Loadout
	{
		private HullKey _hull;
		private List<BuildingKey> _buildings;
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
	}
}