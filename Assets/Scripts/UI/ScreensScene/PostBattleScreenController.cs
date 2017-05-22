using BattleCruisers.Scenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene
{
	// FELIX  Add:
	// + Level name
	// + Item (loot) unlocked => Constant for each level?
	// + Stats?  Time?  Medals earned?
	public class BattleResult
	{
		public int LevelNum { get; private set; }
		public bool WasVictory { get; private set; }

		public BattleResult(int levelNum, bool wasVictory)
		{
			LevelNum = levelNum;
			WasVictory = wasVictory;
		}
	}

	public class PostBattleScreenController : ScreenController
	{
		private BattleResult _battleResult;

		public void Initialise(BattleResult battleResult, ScreensSceneGod screensSceneGod)
		{
			base.Initialise(screensSceneGod);

			Assert.IsNotNull(battleResult);
			_battleResult = battleResult;

			// FELIX  Determine title based on result
			// FELIX  Display loot
		}

		public void Retry()
		{
			Debug.Log("Retry()");
		}

		public void GoToLoadoutScreen()
		{
			_screensSceneGod.GoToLoadoutScreen();
		}

		public void Next()
		{
			Debug.Log("Next()");
		}

		public void GoToHomeScreen()
		{
			_screensSceneGod.GoToHomeScreen();
		}
	}
}