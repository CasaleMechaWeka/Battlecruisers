using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Utils
{
	public class BuildingKey
	{
		public BuildingCategory Category { get; private set; }
		public string PrefabFileName { get; private set; }

		public BuildingKey(BuildingCategory category, string prefabFileName)
		{
			Category = category;
			PrefabFileName = prefabFileName;
		}
	}

	public class UnitKey
	{
		public UnitCategory Category { get; private set; }
		public string PrefabFileName { get; private set; }

		public UnitKey(UnitCategory category, string prefabFileName)
		{
			Category = category;
			PrefabFileName = prefabFileName;
		}
	}

	public class PrefabFetcher
	{
		private const string BUILDINGS_BASE_PATH = "Prefabs/Buildables/Buildings/";
		private const string UNITS_BASE_PATH = "Prefabs/Buildables/Units/";

		private static class BuildingFolderNames
		{
			public const string FACTORIES = "Factories";
			public const string TACTICAL = "Tactical";
			public const string DEFENCE  = "Defence";
			public const string OFFENCE  = "Offence";
		}

		private static class UnitFolderNames
		{
			public const string NAVAL = "Naval";
			public const string AIRCRAFT = "Aircraft";
			public const string ULTRA = "Ultras";
		}

		public BuildingWrapper GetBuildingPrefab(BuildingKey buildingKey)
		{
			string buildingPrefabPath = GetBuildingPath(buildingKey);
			return GetPrefab<BuildingWrapper>(buildingPrefabPath);
		}

		public UnitWrapper GetUnitPrefab(UnitKey unitKey)
		{
			string unitPrefabPath = GetUnitPath(unitKey);
			return GetPrefab<UnitWrapper>(unitPrefabPath);
		}

		private T GetPrefab<T>(string prefabPath)
		{
			GameObject gameObject = Resources.Load(prefabPath) as GameObject;
			if (gameObject == null)
			{
				throw new ArgumentException($"Invalid prefab path: {prefabPath}");
			}

			T prefabObject = gameObject.GetComponent<T>();
			if (prefabObject == null)
			{
				throw new ArgumentException($"Prefab does not contain a component of type: {typeof(T)}.  Prefab path: {prefabPath}");
			}
			return prefabObject;
		}

		private string GetBuildingPath(BuildingKey buildingKey)
		{
			return BUILDINGS_BASE_PATH + GetBuildingFolderName(buildingKey.Category) + "/" + buildingKey.PrefabFileName;
		}

		private string GetUnitPath(UnitKey unitKey)
		{
			return UNITS_BASE_PATH + GetUnitFolderName(unitKey.Category) + "/" + unitKey.PrefabFileName;
		}

		private string GetBuildingFolderName(BuildingCategory buildingCategory)
		{
			switch (buildingCategory)
			{
				case BuildingCategory.Factory:
					return BuildingFolderNames.FACTORIES;
				case BuildingCategory.Tactical:
					return BuildingFolderNames.TACTICAL;
				case BuildingCategory.Defence:
					return BuildingFolderNames.DEFENCE;
				case BuildingCategory.Offence:
					return BuildingFolderNames.OFFENCE;
				default:
					throw new ArgumentException();
			}
		}

		private static string GetUnitFolderName(UnitCategory unitCategory)
		{
			switch (unitCategory)
			{
				case UnitCategory.Aircraft:
					return UnitFolderNames.AIRCRAFT;
				case UnitCategory.Naval:
					return UnitFolderNames.NAVAL;
				default:
					throw new ArgumentException();
			}
		}
	}
}
