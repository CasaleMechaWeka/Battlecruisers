using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Fetchers.PrefabKeys
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

		protected PrefabType PrefabType { private get; set; }
		protected string PrefabTypeCategory { private get; set; }

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
}
