using BattleCruisers.Data;
using BattleCruisers.Scenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ChooseLevelScene
{
	public interface IUIFactory
	{
		void CreateLevelButton(HorizontalOrVerticalLayoutGroup buttonParent, int levelNum, ILevel level, IChooseLevelSceneGod chooseLevelGod);
		void CreateQuitButton(HorizontalOrVerticalLayoutGroup buttonParent, IChooseLevelSceneGod chooseLevelGod);
	}

	public class UIFactory : MonoBehaviour, IUIFactory
	{
		public Button levelButtonPrefab;
		public Button quitButtonPrefab;

		public void CreateLevelButton(HorizontalOrVerticalLayoutGroup buttonParent, int levelNum, ILevel level, IChooseLevelSceneGod chooseLevelGod)
		{
			Button levelButton = (Button)Instantiate(levelButtonPrefab);
			levelButton.transform.SetParent(buttonParent.transform, worldPositionStays: false);
			levelButton.GetComponent<LevelButtonController>().Initialise(levelNum, level, chooseLevelGod);
		}

		public void CreateQuitButton(HorizontalOrVerticalLayoutGroup buttonParent, IChooseLevelSceneGod chooseLevelGod)
		{
			Button quitButton = (Button)Instantiate(quitButtonPrefab);
			quitButton.transform.SetParent(buttonParent.transform, worldPositionStays: false);
			quitButton.GetComponent<QuitButtonController>().Initialise(chooseLevelGod);
		}
	}
}
