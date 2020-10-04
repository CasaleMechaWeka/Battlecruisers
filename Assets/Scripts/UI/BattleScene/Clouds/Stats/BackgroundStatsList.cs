using BattleCruisers.Data.Static;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Clouds.Stats
{
    public class BackgroundStatsList : MonoBehaviour, IBackgroundStatsProvider
    {
        private BackgroundImageStats[] _stats;

        public void Initialise()
        {
            _stats = GetComponentsInChildren<BackgroundImageStats>();
            Assert.AreEqual(StaticData.NUM_OF_LEVELS, _stats.Length);
        }

        public Task<IBackgroundImageStats> GetStatsAsync(int levelNum)
        {
            int index = levelNum - 1;
            Assert.IsTrue(index >= 0);
            Assert.IsTrue(index < _stats.Length);

            return Task.FromResult((IBackgroundImageStats)_stats[index]);
        }
    }
}