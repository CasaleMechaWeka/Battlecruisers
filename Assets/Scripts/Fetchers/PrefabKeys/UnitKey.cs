using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Fetchers.PrefabKeys
{
	public class UnitKey : BuildableKey
	{
		private static class UnitFolderNames
		{
			public const string NAVAL = "Naval";
			public const string AIRCRAFT = "Aircraft";
			public const string ULTRA = "Ultras";
		}

		public UnitCategory UnitCategory { get; private set; }

		protected override string PrefabPathPrefix
		{
			get
			{
				return base.PrefabPathPrefix + UnitCategoryToFolderName(UnitCategory) + PATH_SEPARATOR;
			}
		}

		public UnitKey(UnitCategory category, string prefabFileName)
			: base(prefabFileName, BuildableType.Unit)
		{
			UnitCategory = category;
		}

		private string UnitCategoryToFolderName(UnitCategory unitCategory)
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
