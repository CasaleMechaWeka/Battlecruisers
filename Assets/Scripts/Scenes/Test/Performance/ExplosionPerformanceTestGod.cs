using BattleCruisers.Data.Static;
using BattleCruisers.Effects.Explosions;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Utils.Fetchers;
using System.Collections.Generic;
using UnityEngine;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Performance
{
    public class ExplosionPerformanceTestGod : TestGodBase
    {
        private IPrefabFactory _prefabFactory;
        private BCUtils.IRandomGenerator _random;

        public float spawnRadiusXInM = 8;
        public float spawnRadiusYInM = 5;

        protected override List<GameObject> GetGameObjects()
        {
            return new List<GameObject>()
            {
                gameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            _prefabFactory = helper.PrefabFactory;
            _random = BCUtils.RandomGenerator.Instance;
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