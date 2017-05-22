using BattleCruisers.Data;
using BattleCruisers.Scenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
	public class HomeButtonController : MonoBehaviour 
	{
		public Button button;

		public void Initialise(IScreensSceneGod screensSceneGod)
		{
			button.onClick.AddListener(() => screensSceneGod.GoToHomeScreen());
		}
	}
}
