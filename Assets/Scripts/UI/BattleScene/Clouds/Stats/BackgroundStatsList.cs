using BattleCruisers.Data.Static;
using BattleCruisers.Utils.Fetchers;
using System.Linq;
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

        public Task<IPrefabContainer<BackgroundImageStats>> GetStatsAsyncLevel(int levelNum)
        {
            Debug.Log(_stats.Count());
            int index = levelNum - 1;
            Assert.IsTrue(index >= 0);
            Assert.IsTrue(index < _stats.Length);

            IPrefabContainer<BackgroundImageStats> result = new NullPrefabContainer<BackgroundImageStats>()
            {
                Prefab = _stats[index]
            };

            return Task.FromResult(result);
        }

        public Task<IPrefabContainer<BackgroundImageStats>> GetStatsAsyncSideQuest(int sideQuestID)
        {
            int index = sideQuestID - 1;
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