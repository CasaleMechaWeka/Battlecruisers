using BattleCruisers.Data.Static;
using BattleCruisers.Effects.Explosions;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Performance
{
    public class ExplosionPerformanceTestGod : MonoBehaviour
    {
        private IPrefabFactory _prefabFactory;
        private BCUtils.IRandomGenerator _random;

        public float spawnRadiusXInM = 8;
        public float spawnRadiusYInM = 5;

        async void Start()
        {
            _prefabFactory = await Helper.CreatePrefabFactoryAsync();
            _random = BCUtils.RandomGenerator.Instance;
        }

        private void Update()
        {
            if (_prefabFactory == null)
            {
                // Async initialistion not complete
                return;
            }

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