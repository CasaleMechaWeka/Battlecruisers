using BattleCruisers.Buildables.Buildings;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Fetchers
{
	public enum PrefabType
	{
		Hull, Building, Unit
	}

	public interface IPrefabKey
	{
		string PrefabPath { get; }
	}

	public abstract class PrefabKey : IPrefabKey
	{
		private static class PrefabTypeFolderNames
		{
			public const string HULLS = "Hulls";
			public const string UNITS = "Units";
			public const string BUILDINGS = "Buildings";
		}

		private string _prefabName;

		private const string PREFABS_BASE_PATH = "Prefabs/Buildables/";
		private const char PATH_SEPARATOR = '/';

		public string PrefabPath
		{
			get
			{
				string prefabPath = PREFABS_BASE_PATH + PrefabType + PATH_SEPARATOR;
				
				if (PrefabTypeCategory != null)
				{
					prefabPath += PrefabTypeCategory + PATH_SEPARATOR;
				}
				prefabPath += _prefabName;
				
				return prefabPath;
			}
		}

		protected abstract PrefabType PrefabType { get; set; }
		protected virtual string PrefabTypeCategory { get; set; }

		public PrefabKey(string prefabName)
		{
			_prefabName = prefabName;
			PrefabTypeCategory = null;
		}

		private string GetPrefabTypeFolderName(PrefabType prefabType)
		{
			switch (prefabType)
			{
				case PrefabType.Hull:
					return PrefabTypeFolderNames.HULLS;
				case PrefabType.Unit:
					return PrefabTypeFolderNames.UNITS;
				case PrefabType.Building:
					return PrefabTypeFolderNames.BUILDINGS;
				default:
					throw new ArgumentException();
			}
		}
	}

	public class BuildingKey : PrefabKey
	{
		private static class BuildingFolderNames
		{
			public const string FACTORIES = "Factories";
			public const string TACTICAL = "Tactical";
			public const string DEFENCE  = "Defence";
			public const string OFFENCE  = "Offence";
		}

		protected override PrefabType PrefabType { get; set; }
		protected override string PrefabTypeCategory { get; set; }

		public BuildingKey(BuildingCategory category, string prefabFileName)
			: base(prefabFileName)
		{
			PrefabType = PrefabType.Building;
			PrefabTypeCategory = GetBuildingFolderName(category);
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
	}
}
