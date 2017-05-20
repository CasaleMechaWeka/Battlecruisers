using BattleCruisers.Fetchers.PrefabKeys;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.DataModel
{
	public interface IDataProvider
	{
		IList<UnitKey> GetAllUnits();
		IList<UnitKey> GetAllUnlockedUnits();

		IList<BuildingKey> GetAllBuildings();
		IList<BuildingKey> GetAllUnlockedBuildings();

		IList<HullKey> GetAllHulls();
		IList<HullKey> GetAllUnlockedHulls();
	}

	public class DataProvider : IDataProvider
	{
		public IList<UnitKey> GetAllUnits()
		{
			throw new NotImplementedException();
		}

		public IList<UnitKey> GetAllUnlockedUnits()
		{
			throw new NotImplementedException();
		}

		public IList<BuildingKey> GetAllBuildings()
		{
			throw new NotImplementedException();
		}

		public IList<BuildingKey> GetAllUnlockedBuildings()
		{
			throw new NotImplementedException();
		}

		public IList<HullKey> GetAllHulls()
		{
			throw new NotImplementedException();
		}

		public IList<HullKey> GetAllUnlockedHulls()
		{
			throw new NotImplementedException();
		}
	}
}
