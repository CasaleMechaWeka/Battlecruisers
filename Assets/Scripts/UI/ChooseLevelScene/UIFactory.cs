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
		void CreateBackButton(HorizontalOrVerticalLayoutGroup buttonParent, IChooseLevelSceneGod chooseLevelGod);
	}

	public class UIFactory : MonoBehaviour, IUIFactory
	{
		public Button levelButtonPrefab;
		public Button backButtonPrefab;

		public void CreateLevelButton(HorizontalOrVerticalLayoutGroup buttonParent, int levelNum, ILevel level, IChooseLevelSceneGod chooseLevelGod)
		{
			Button levelButton = (Button)Instantiate(levelButtonPrefab);
			levelButton.transform.SetParent(buttonParent.transform, worldPositionStays: false);
			levelButton.GetComponent<LevelButtonController>().Initialise(levelNum, level, chooseLevelGod);
		}

		public void CreateBackButton(HorizontalOrVerticalLayoutGroup buttonParent, IChooseLevelSceneGod chooseLevelGod)
		{
			Button backButton = (Button)Instantiate(backButtonPrefab);
			backButton.transform.SetParent(buttonParent.transform, worldPositionStays: false);
			backButton.GetComponent<BackButtonController>().Initialise(chooseLevelGod);
		}
	}
}
