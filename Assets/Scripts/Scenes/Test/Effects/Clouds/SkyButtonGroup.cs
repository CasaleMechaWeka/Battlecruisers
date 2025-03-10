using BattleCruisers.UI.BattleScene.Clouds.Stats;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Effects.Clouds
{
    public class SkyButtonGroup : MonoBehaviour
    {
        public void Initialise(ISkySetter skySetter, IList<ISkyStats> skyStats)
        {
            BCUtils.Helper.AssertIsNotNull(skySetter, skyStats);

            SkyButtonController[] buttons = GetComponentsInChildren<SkyButtonController>();
            Assert.AreEqual(buttons.Length, skyStats.Count);

            for (int i = 0; i < buttons.Length; ++i)
            {
                buttons[i].Initialise(skySetter, skyStats[i]);
            }
        }
    }
}