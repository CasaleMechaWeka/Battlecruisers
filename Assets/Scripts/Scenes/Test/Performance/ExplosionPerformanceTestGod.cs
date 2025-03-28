using BattleCruisers.Data.Static;
using BattleCruisers.Utils.Fetchers;
using System.Collections.Generic;
using UnityEngine;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Performance
{
    public class ExplosionPerformanceTestGod : TestGodBase
    {

        public float spawnRadiusXInM = 8;
        public float spawnRadiusYInM = 5;

        protected override List<GameObject> GetGameObjects()
        {
            return new List<GameObject>()
            {
                gameObject
            };
        }

        private void Update()
        {
            IPoolable<Vector3> explosion = PrefabFactory.CreateExplosion(StaticPrefabKeys.Explosions.FlakExplosion);
            explosion.Activate(FindRandomSpawnPosition());
        }

        private Vector3 FindRandomSpawnPosition()
        {
            float x = RandomGenerator.Range(-spawnRadiusXInM, spawnRadiusXInM);
            float y = RandomGenerator.Range(-spawnRadiusYInM, spawnRadiusYInM);
            return new Vector3(x, y, 0);
        }
    }
}