using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers
{
	public class Loadout
	{
		// FELIX  Cruiser hull
		IDictionary<BuildingCategory, IList<BuildingKey>> _buildings;
		IDictionary<UnitCategory, IList<UnitKey>> _units;

		public Loadout(
			IList<BuildingKey> factories,
			IList<BuildingKey> tacticals,
			IList<BuildingKey> defence,
			IList<BuildingKey> offence,
			IList<BuildingKey> support,
			IList<UnitKey> aircraft,
			IList<UnitKey> ships)
		{
			_buildings = new Dictionary<BuildingCategory, IList<BuildingKey>>();
			_buildings[BuildingCategory.Factory] = factories;
			_buildings[BuildingCategory.Tactical] = tacticals;
			_buildings[BuildingCategory.Defence] = defence;
			_buildings[BuildingCategory.Offence] = offence;
			_buildings[BuildingCategory.Support] = support;

			_units = new Dictionary<UnitCategory, IList<UnitKey>>();
			_units[UnitCategory.Aircraft] = aircraft;
			_units[UnitCategory.Naval] = ships;
		}

		public IList<BuildingKey> GetBuildings(BuildingCategory buildingCategory)
		{
			return _buildings[buildingCategory];
		}

		public IList<UnitKey> GetUnits(UnitCategory unitCategory)
		{
			return _units[unitCategory];
		}
	}
}