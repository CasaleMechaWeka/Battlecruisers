using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Data.PrefabKeys
{
	[Serializable]
	public class BuildingKey : BuildableKey
	{
		private static class BuildingFolderNames
		{
			public const string FACTORIES = "Factories";
			public const string TACTICAL = "Tactical";
			public const string DEFENCE  = "Defence";
			public const string OFFENCE  = "Offence";
			public const string ULTRAS  = "Ultras";
		}

		[SerializeField]
		private BuildingCategory _buildingCategory;

		public BuildingCategory BuildingCategory
		{ 
			get { return _buildingCategory; }
			private set { _buildingCategory = value; }
		}

		protected override string PrefabPathPrefix
		{
			get
			{
				return base.PrefabPathPrefix + BuildingCategoryToFolderName(BuildingCategory) + PATH_SEPARATOR;
			}
		}

		public BuildingKey(BuildingCategory category, string prefabFileName)
			: base(prefabFileName, BuildableType.Building)
		{
			BuildingCategory = category;
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
				case BuildingCategory.Ultra:
					return BuildingFolderNames.ULTRAS;
				default:
					throw new ArgumentException();
			}
		}
	}
}
