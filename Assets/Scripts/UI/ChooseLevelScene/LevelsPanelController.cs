using BattleCruisers.Data;
using BattleCruisers.Scenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ChooseLevelScene
{
	public class LevelsPanelController : MonoBehaviour 
	{
		private int _levelNum;
		private IChooseLevelSceneGod _chooseLevelGod;

		public HorizontalOrVerticalLayoutGroup buttonsWrapper;

		public void Initialise(IUIFactory uiFactory, IChooseLevelSceneGod chooseLevelGod, IList<ILevel> levels)
		{
			for (int i = 0; i < levels.Count; ++i)
			{
				int levelNum = i + 1;
				uiFactory.CreateLevelButton(buttonsWrapper, levelNum, levels[i], chooseLevelGod); 
			}
		}
	}
}
