using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Clouds.Stats
{
    public class BackgroundStatsList : MonoBehaviour
    {
        private BackgroundImageStats[] _stats;

        public void Initialise()
        {
            _stats = GetComponentsInChildren<BackgroundImageStats>();
            Assert.AreEqual(25, _stats.Length);
        }

        public IBackgroundImageStats GetStats(int levelNum)
        {
            int index = levelNum - 1;
            Assert.IsTrue(index >= 0);
            Assert.IsTrue(index < _stats.Length);

            return _stats[index];
        }
    }
}