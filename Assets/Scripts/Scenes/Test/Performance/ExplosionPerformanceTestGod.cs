using BattleCruisers.Data.Static;
using BattleCruisers.Effects.Explosions;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Performance
{
    public class ExplosionPerformanceTestGod : MonoBehaviour
    {
        private IPrefabFactory _prefabFactory;
        private IRandomGenerator _random;

        public float spawnRadiusXInM = 8;
        public float spawnRadiusYInM = 5;

        void Start()
        {
            _prefabFactory = new PrefabFactory(new PrefabFetcherLEGACY());
            _random = RandomGenerator.Instance;
        }

        private void Update()
        {
            IExplosion explosion = _prefabFactory.CreateExplosion(StaticPrefabKeys.Explosions.Explosion75);
            explosion.Activate(FindRandomSpawnPosition());
        }

        private Vector3 FindRandomSpawnPosition()
        {
            float x = _random.Range(-spawnRadiusXInM, spawnRadiusXInM);
            float y = _random.Range(-spawnRadiusYInM, spawnRadiusYInM);
            return new Vector3(x, y, 0);
        }
    }
}