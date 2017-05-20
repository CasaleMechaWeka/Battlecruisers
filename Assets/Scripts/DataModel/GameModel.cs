using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.DataModel
{
	public interface IGameModel
	{
		int NumOfLevelsUnlocked { get; set; }
		Loadout PlayerLoadout { get; set; }
	}

	[Serializable]
	public class GameModel : IGameModel
	{
		[SerializeField]
		private int _numOfLevelsUnlocked;

		[SerializeField]
		private Loadout _playerLoadout;

		public int NumOfLevelsUnlocked 
		{ 
			get { return _numOfLevelsUnlocked; }
			set { _numOfLevelsUnlocked = value; }
		}

		public Loadout PlayerLoadout
		{
			get { return _playerLoadout; }
			set { _playerLoadout = value; }
		}

		public override bool Equals(object obj)
		{
			GameModel other = obj as GameModel;

			return other != null
				&& other.NumOfLevelsUnlocked == NumOfLevelsUnlocked
				&& object.ReferenceEquals(PlayerLoadout, other.PlayerLoadout) ||
					(PlayerLoadout != null && PlayerLoadout.Equals(other.PlayerLoadout));
		}

		public override int GetHashCode()
		{
			return this.GetHashCode(NumOfLevelsUnlocked, PlayerLoadout);
		}
	}
}
