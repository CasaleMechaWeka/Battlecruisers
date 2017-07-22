using System;
using BattleCruisers.Buildables.Units;
using UnityEngine;

namespace BattleCruisers.Data.PrefabKeys
{
    [Serializable]
	public class UnitKey : BuildableKey
	{
		private static class UnitFolderNames
		{
			public const string NAVAL = "Naval";
			public const string AIRCRAFT = "Aircraft";
			public const string ULTRA = "Ultras";
		}

		[SerializeField]
		private UnitCategory _unitCategory;

		public UnitCategory UnitCategory
		{ 
			get { return _unitCategory; }
			private set { _unitCategory = value; }
		}

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
