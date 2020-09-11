using UnityEngine;
using System.Collections;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.BattleScene.Clouds.Stats;
using UnityEngine.UI;
using UnityEngine.Assertions;
using BattleCruisers.Data;

namespace Assets.Scripts.Scenes.Test.Utilities
{
    public class LevelButtonController : MonoBehaviour
    {
        public Text levelNumText;

        public void Initialise(int levelNum, SkyStatsGroup skyStatsGroup, IStaticData staticData)
        {
            Assert.IsNotNull(levelNumText);

            levelNumText.text = levelNum.ToString();

            ILevel level = staticData.Levels[levelNum - 1];
            // FELIX  TODO :)
        }
    }
}