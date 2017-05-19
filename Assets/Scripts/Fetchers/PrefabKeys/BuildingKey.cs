using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Fetchers.PrefabKeys
{
	public class BuildingKey : BuildableKey
	{
		private static class BuildingFolderNames
		{
			public const string FACTORIES = "Factories";
			public const string TACTICAL = "Tactical";
			public const string DEFENCE  = "Defence";
			public const string OFFENCE  = "Offence";
		}

		private BuildingCategory _buildingCategory;

		protected override string PrefabPathPrefix
		{
			get
			{
				return base.PrefabPathPrefix + BuildingCategoryToFolderName(_buildingCategory) + PATH_SEPARATOR;
			}
		}

		public BuildingKey(BuildingCategory category, string prefabFileName)
			: base(prefabFileName, BuildableType.Building)
		{
			_buildingCategory = category;
		}

		private string BuildingCategoryToFolderName(BuildingCategory buildingCategory)
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
