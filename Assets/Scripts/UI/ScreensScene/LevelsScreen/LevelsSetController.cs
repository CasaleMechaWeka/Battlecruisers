using System.Collections;
using System.Collections.Generic;
using BattleCruisers.Data;
using BattleCruisers.Scenes;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    public class LevelsSetController : MonoBehaviour
    {
		public void Initialise(IScreensSceneGod screensSceneGod, UIFactory uiFactory, IList<ILevel> levels, int numOfLevelsUnlocked)
        {
            HorizontalOrVerticalLayoutGroup buttonsWrapper = GetComponent<HorizontalOrVerticalLayoutGroup>();
            Assert.IsNotNull(buttonsWrapper);

            // Create level buttons
            foreach (ILevel level in levels)
			{
                bool isLevelUnlocked = level.Num <= numOfLevelsUnlocked;
			    uiFactory.CreateLevelButton(buttonsWrapper, level, isLevelUnlocked, screensSceneGod); 
			}
		}
	}
}
