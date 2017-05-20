using BattleCruisers.Utils;
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
		public Loadout playerLoadout;

		public override bool Equals(object obj)
		{
			GameModel other = obj as GameModel;

			return other != null
				&& other.numOfLevelsUnlocked == numOfLevelsUnlocked
				&& object.ReferenceEquals(playerLoadout, other.playerLoadout) ||
					(playerLoadout != null && playerLoadout.Equals(other.playerLoadout));
		}

		public override int GetHashCode()
		{
			return this.GetHashCode(numOfLevelsUnlocked, playerLoadout);
		}
	}
}
