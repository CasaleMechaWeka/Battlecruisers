using BattleCruisers.Data.Static;
using BattleCruisers.Utils.Fetchers;
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

        public Task<IPrefabContainer<BackgroundImageStats>> GetStatsAsync(int levelNum)
        {
            int index = levelNum - 1;
            Assert.IsTrue(index >= 0);
            Assert.IsTrue(index < _stats.Length);

            IPrefabContainer<BackgroundImageStats> result = new NullPrefabContainer<BackgroundImageStats>()
            {
                Prefab = _stats[index]
            };

            return Task.FromResult(result);
        }
    }
}