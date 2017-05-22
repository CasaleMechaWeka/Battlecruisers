using BattleCruisers.Data;
using BattleCruisers.Scenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene
{
	public class LevelsPanelController : MonoBehaviour 
	{
		private int _levelNum;
		private IScreensSceneGod _screensSceneGod;

		public HorizontalOrVerticalLayoutGroup buttonsWrapper;

		public void Initialise(IUIFactory uiFactory, IScreensSceneGod screensSceneGod, IList<ILevel> levels)
		{
			// Create level buttons
			for (int i = 0; i < levels.Count; ++i)
			{
				int levelNum = i + 1;
				uiFactory.CreateLevelButton(buttonsWrapper, levelNum, levels[i], screensSceneGod); 
			}

			uiFactory.CreateBackButton(buttonsWrapper, screensSceneGod);
		}
	}
}
