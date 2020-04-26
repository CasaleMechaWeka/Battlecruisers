using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Clouds.Stats
{
    public class SkyStatsGroup : MonoBehaviour
    {
        private const int EXPECTED_NUMBER_OF_SKIES = 7;

        public IList<ISkyStats> SkyStats { get; private set; }

        public void Initialise()
        {
            SkyStatsController[] skyStats = GetComponentsInChildren<SkyStatsController>();
            Assert.AreEqual(EXPECTED_NUMBER_OF_SKIES, skyStats.Length);

            SkyStats = new List<ISkyStats>();
            foreach (SkyStatsController stats in skyStats)
            {
                stats.Initialise();
                SkyStats.Add(stats);
            }
        }

        public ISkyStats GetSkyStats(string skyMaterialName)
        {
            ISkyStats skyStats = SkyStats.FirstOrDefault(stats => stats.SkyMaterial.name == skyMaterialName);
            Assert.IsNotNull(skyStats, $"Unknown sky material name: {skyMaterialName}");
            return skyStats;
        }
    }
}