using System.Collections.Generic;
using System.Linq;
using BattleCruisers.UI.BattleScene.Clouds.Stats;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Clouds.Stats
{
    public class PvPSkyStatsGroup : MonoBehaviour
    {
        private const int EXPECTED_NUMBER_OF_SKIES = 7;

        public IList<ISkyStats> SkyStats { get; private set; }

        public void Initialise()
        {
            PvPSkyStatsController[] skyStats = GetComponentsInChildren<PvPSkyStatsController>();
            Assert.AreEqual(EXPECTED_NUMBER_OF_SKIES, skyStats.Length);

            SkyStats = new List<ISkyStats>();
            foreach (PvPSkyStatsController stats in skyStats)
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