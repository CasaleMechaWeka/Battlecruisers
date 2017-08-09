using System.Collections.Generic;
using BattleCruisers.Data;
using BattleCruisers.Scenes;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    public class LevelsScreenController : ScreenController
	{
        private IList<LevelsSetController> _levelSets;
        private int _visibleSetIndex;

        private const int SET_SIZE = 7;

        private LevelsSetController VisibleLevelsSet { get { return _levelSets[_visibleSetIndex]; } }

		public void Initialise(IScreensSceneGod screensSceneGod, IList<ILevel> levels, int numOfLevelsUnlocked)
		{
			base.Initialise(screensSceneGod);

            UIFactory uiFactory = GetComponent<UIFactory>();
            Assert.IsNotNull(uiFactory);

            Assert.IsTrue(levels.Count % SET_SIZE == 0);
            int numOfSets = levels.Count / SET_SIZE;
            _levelSets = new List<LevelsSetController>(numOfSets);

            for (int j = 0; j < numOfSets; j++)
            {
                IList<ILevel> setLevels = new List<ILevel>(SET_SIZE);

                for (int i = 0; i < SET_SIZE; ++i)
                {
                    setLevels.Add(levels[j * SET_SIZE + i]);
                }

                LevelsSetController levelsSet = uiFactory.CreateLevelsSet(screensSceneGod, this, uiFactory, setLevels, numOfLevelsUnlocked);
                levelsSet.gameObject.SetActive(false);
                _levelSets.Add(levelsSet);
            }

            // FELIX  Focus on set which had the last played level, not just hardcoded :P
            _visibleSetIndex = 1;
            ShowSet(_visibleSetIndex);
        }

        private void ShowSet(int setIndex)
        {
            Assert.IsTrue(setIndex >= 0 && setIndex < _levelSets.Count);

            VisibleLevelsSet.gameObject.SetActive(false);
            _visibleSetIndex = setIndex;
            VisibleLevelsSet.gameObject.SetActive(true);
        }
	}
}
