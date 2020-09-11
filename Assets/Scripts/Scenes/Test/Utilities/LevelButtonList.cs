using BattleCruisers.Data.Static;
using BattleCruisers.UI.BattleScene.Clouds.Stats;
using UnityEngine;
using UnityEngine.Assertions;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Utilities
{
    public class LevelButtonList : MonoBehaviour
    {
        private const int NUM_OF_LEVELS = 25;

        public void Initialise(SkyStatsGroup skyStats, ISkySetter skySetter, IStaticData staticData)
        {
            BCUtils.Helper.AssertIsNotNull(skyStats, skySetter, staticData);

            LevelButtonController[] buttons = GetComponentsInChildren<LevelButtonController>();
            Assert.AreEqual(NUM_OF_LEVELS, buttons.Length);

            for (int i = 0; i < buttons.Length; ++i)
            {
                int levelNum = i + 1;
                buttons[i].Initialise(levelNum, skyStats, skySetter, staticData);
            }
        }
    }
}