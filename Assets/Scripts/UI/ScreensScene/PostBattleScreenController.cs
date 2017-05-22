using BattleCruisers.Scenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

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

		public Text title;
		public GameObject unlockedItemSection;

		private const string VICTORY_TITLE = "Congratulations!";
		private const string LOSS_TITLE = "Bad luck!";

		public void Initialise(BattleResult battleResult, ScreensSceneGod screensSceneGod)
		{
			base.Initialise(screensSceneGod);

			Assert.IsNotNull(battleResult);
			_battleResult = battleResult;

			title.text = _battleResult.WasVictory ? VICTORY_TITLE : LOSS_TITLE;
			unlockedItemSection.SetActive(_battleResult.WasVictory);
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