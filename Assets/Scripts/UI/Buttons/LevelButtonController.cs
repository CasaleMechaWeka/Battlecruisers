using BattleCruisers.Data;
using BattleCruisers.Scenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.Buttons
{
	public class LevelButtonController : MonoBehaviour 
	{
		private int _levelNum;
		private IChooseLevelSceneGod _chooseLevelGod;

		public Button button;
		public Text levelName;

		public void Initialise(int levelNum, ILevel level, IChooseLevelSceneGod chooseLevelGod)
		{
			_levelNum = levelNum;
			_chooseLevelGod = chooseLevelGod;			

			levelName.text = _levelNum + ". " + level.Name;
			button.onClick.AddListener(ChooseLevel);
		}

		private void ChooseLevel()
		{
			_chooseLevelGod.LoadLevel(_levelNum);
		}
	}
}
