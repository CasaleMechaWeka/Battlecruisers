using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.DataModel
{
	[Serializable]
	public class GameModel
	{
		public int numOfLevelsUnlocked;

		public override bool Equals(object obj)
		{
			GameModel other = obj as GameModel;
			return other != null
			&& other.numOfLevelsUnlocked == numOfLevelsUnlocked;
		}

		public override int GetHashCode()
		{
			int hash = 17;

			hash *= 23 * numOfLevelsUnlocked.GetHashCode();

			return hash;
		}
	}
}
