using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Fetchers.PrefabKeys
{
	public class HullKey : PrefabKey
	{
		private const string HULLS_FOLDER_NAME = "Hulls";

		protected override string PrefabPathPrefix
		{
			get
			{
				return base.PrefabPathPrefix + HULLS_FOLDER_NAME + PATH_SEPARATOR;
			}
		}

		public HullKey(string prefabFileName)
			: base(prefabFileName) { }
	}
}
