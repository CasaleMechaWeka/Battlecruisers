using BattleCruisers.Data;
using BattleCruisers.Scenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ChooseLevelScene
{
	public class QuitButtonController : MonoBehaviour 
	{
		public Button button;

		public void Initialise(IChooseLevelSceneGod chooseLevelGod)
		{
			button.onClick.AddListener(() => chooseLevelGod.Quit());
		}
	}
}
