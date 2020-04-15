using BattleCruisers.UI.BattleScene.Clouds;
using BattleCruisers.UI.BattleScene.Clouds.Stats;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Utilities
{
    public class SkyButtonGroup : MonoBehaviour
    {
        public Skybox skybox;
        public List<CloudController> clouds;
        public MistController mist;

        public void Initialise(IList<ISkyStats> skyStats)
        {
            Assert.IsNotNull(skyStats);
            BCUtils.Helper.AssertIsNotNull(skybox, clouds, mist);

            IList<ICloud> cloudList
                = clouds
                    .Select(cloud => (ICloud)cloud)
                    .ToList();

            SkyButtonController[] buttons = GetComponentsInChildren<SkyButtonController>();
            Assert.AreEqual(buttons.Length, skyStats.Count);

            for (int i = 0; i < buttons.Length; ++i)
            {
                buttons[i].Initialise(skybox, skyStats[i], cloudList, mist);
            }
        }
    }
}