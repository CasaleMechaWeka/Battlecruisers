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
		Button CreateLevelButton(HorizontalOrVerticalLayoutGroup buttonParent, int levelNum, ILevel level, IChooseLevelSceneGod chooseLevelGod);
	}

	public class UIFactory : MonoBehaviour, IUIFactory
	{
		public Button levelButtonPrefab;

		public Button CreateLevelButton(HorizontalOrVerticalLayoutGroup buttonParent, int levelNum, ILevel level, IChooseLevelSceneGod chooseLevelGod)
		{
			Button levelButton = (Button)Instantiate(levelButtonPrefab);
			levelButton.transform.SetParent(buttonParent.transform, worldPositionStays: false);
			levelButton.GetComponent<LevelButtonController>().Initialise(levelNum, level, chooseLevelGod);
			return levelButton;
		}
	}
}
