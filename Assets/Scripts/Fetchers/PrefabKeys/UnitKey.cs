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

		private UnitCategory _unitCategory;

		protected override string PrefabPathPrefix
		{
			get
			{
				return base.PrefabPathPrefix + UnitCategoryToFolderName(_unitCategory) + PATH_SEPARATOR;
			}
		}

		public UnitKey(UnitCategory category, string prefabFileName)
			: base(prefabFileName, BuildableType.Unit)
		{
			_unitCategory = category;
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
