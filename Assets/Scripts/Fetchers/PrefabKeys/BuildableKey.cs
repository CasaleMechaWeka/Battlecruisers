using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Fetchers.PrefabKeys
{
	public enum BuildableType
	{
		Building, Unit
	}

	[Serializable]
	public abstract class BuildableKey : PrefabKey
	{
		private static class BuildableTypeFolderNames
		{
			public const string UNITS = "Units";
			public const string BUILDINGS = "Buildings";
		}

		[SerializeField]
		private BuildableType _buildableType;

		private const string BUILDABLES_FOLDER_NAME = "Buildables";

		protected override string PrefabPathPrefix
		{
			get
			{
				return base.PrefabPathPrefix + BUILDABLES_FOLDER_NAME + PATH_SEPARATOR + BuildableTypeToFolderName(_buildableType) + PATH_SEPARATOR;
			}
		}

		public BuildableKey(string prefabFileName, BuildableType buildableType)
			: base(prefabFileName)
		{
			_buildableType = buildableType;
		}

		private string BuildableTypeToFolderName(BuildableType buildableType)
		{
			switch (buildableType)
			{
				case BuildableType.Building:
					return BuildableTypeFolderNames.BUILDINGS;
				case BuildableType.Unit:
					return BuildableTypeFolderNames.UNITS;
				default:
					throw new ArgumentException();
			}
		}
	}
}
