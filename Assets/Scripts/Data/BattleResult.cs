using System;
using UnityEngine;

namespace BattleCruisers.Data
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
	}
}
