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
		public HullKey(string prefabFileName)
			: base(prefabFileName)
		{
			PrefabType = PrefabType.Hull;
		}
	}
}
