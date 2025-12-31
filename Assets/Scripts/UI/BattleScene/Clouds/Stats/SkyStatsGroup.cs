using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Clouds.Stats
{
    public class SkyStatsGroup : MonoBehaviour
    {
        private const int EXPECTED_NUMBER_OF_SKIES = 7;

        public IList<SkyStatsController> SkyStats { get; private set; }

        public void Initialise()
        {
            SkyStatsController[] skyStats = GetComponentsInChildren<SkyStatsController>();
            Assert.AreEqual(EXPECTED_NUMBER_OF_SKIES, skyStats.Length);

            SkyStats = new List<SkyStatsController>();
            foreach (SkyStatsController stats in skyStats)
            {
                stats.Initialise();
                SkyStats.Add(stats);
            }
        }

        public SkyStatsController GetSkyStats(string skyMaterialName)
        {
            SkyStatsController skyStats = SkyStats.FirstOrDefault(stats => stats.SkyMaterial.name == skyMaterialName);
            Assert.IsNotNull(skyStats, $"Unknown sky material name: {skyMaterialName}");
            return skyStats;
        }
    }
}