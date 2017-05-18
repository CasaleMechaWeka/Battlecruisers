using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Fetchers.PrefabKeys
{
	public class BuildingKey : PrefabKey
	{
		private static class BuildingFolderNames
		{
			public const string FACTORIES = "Factories";
			public const string TACTICAL = "Tactical";
			public const string DEFENCE  = "Defence";
			public const string OFFENCE  = "Offence";
		}

		public BuildingKey(BuildingCategory category, string prefabFileName)
			: base(prefabFileName)
		{
			PrefabType = PrefabType.Building;
			PrefabTypeCategoryFolderName = GetBuildingFolderName(category);
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
