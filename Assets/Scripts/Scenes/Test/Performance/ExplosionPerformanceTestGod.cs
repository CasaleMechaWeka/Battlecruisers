using BattleCruisers.Effects.Explosions;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Performance
{
    public class ExplosionPerformanceTestGod : MonoBehaviour
    {
        private IPrefabFactory _prefabFactory;
        private IExplosionStats _explosionStats;
        private IRandomGenerator _random;

        public ExplosionSize explosionSize = ExplosionSize.Small;
        public float spawnRadiusXInM = 8;
        public float spawnRadiusYInM = 5;

        void Start()
        {
            _prefabFactory = new PrefabFactory(new PrefabFetcher());
            _explosionStats = new ExplosionStats(explosionSize, showTrails: true);
            _random = new RandomGenerator();
        }

        private void Update()
        {
            IExplosion explosion = _prefabFactory.CreateExplosion(_explosionStats);
            explosion.Show(FindRandomSpawnPosition());
        }

        private Vector3 FindRandomSpawnPosition()
        {
            float x = _random.Range(-spawnRadiusXInM, spawnRadiusXInM);
            float y = _random.Range(-spawnRadiusYInM, spawnRadiusYInM);
            return new Vector3(x, y, 0);
        }
    }
}