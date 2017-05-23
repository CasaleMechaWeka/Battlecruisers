using BattleCruisers.Data;
using BattleCruisers.Scenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene
{
	public class PostBattleScreenController : ScreenController
	{
		private BattleResult _battleResult;
		private int _numOfLevelsUnlocked;

		public Text title;
		public GameObject unlockedItemSection;
		public Button nextButton;

		private const string VICTORY_TITLE = "Congratulations!";
		private const string LOSS_TITLE = "Bad luck!";

		public void Initialise(BattleResult battleResult, ScreensSceneGod screensSceneGod, int numOfLevelsUnlocked)
		{
			base.Initialise(screensSceneGod);

			Assert.IsNotNull(battleResult);

			_battleResult = battleResult;
			_numOfLevelsUnlocked = numOfLevelsUnlocked;

			if (_battleResult.WasVictory)
			{
				title.text = VICTORY_TITLE;
			}
			else
			{
				title.text = LOSS_TITLE;

				unlockedItemSection.SetActive(false);

				if (battleResult.LevelNum + 1 > numOfLevelsUnlocked)
				{
					nextButton.gameObject.SetActive(false);
				}
			}
		}

		public void Retry()
		{
			_screensSceneGod.LoadLevel(_battleResult.LevelNum);
		}

		public void GoToLoadoutScreen()
		{
			_screensSceneGod.GoToLoadoutScreen();
		}

		public void Next()
		{
			int nextLevelNum = _battleResult.LevelNum + 1;
			Assert.IsTrue(nextLevelNum <= _numOfLevelsUnlocked);
			_screensSceneGod.LoadLevel(nextLevelNum);
		}

		public void GoToHomeScreen()
		{
			_screensSceneGod.GoToHomeScreen();
		}
	}
}