using BattleCruisers.Utils;
using System;
using UnityEngine;

namespace BattleCruisers.Data.Models
{
	[Serializable]
	public class BattleResult
	{
		[SerializeField]
		private int _levelNum;

		[SerializeField]
		private bool _wasVictory;

		public int LevelNum 
		{
			get { return _levelNum; }
			set { _levelNum = value; }
		}

		public bool WasVictory
		{
			get { return _wasVictory; }
			set { _wasVictory = value; }
		}

		public BattleResult(int levelNum, bool wasVictory)
		{
			LevelNum = levelNum;
			WasVictory = wasVictory;
		}

		public override bool Equals(object obj)
		{
			BattleResult other = obj as BattleResult;
			return other != null
				&& other.LevelNum == LevelNum
				&& other.WasVictory == WasVictory;
		}

		public override int GetHashCode()
		{
			return this.GetHashCode(LevelNum, WasVictory);
		}
	}
}
