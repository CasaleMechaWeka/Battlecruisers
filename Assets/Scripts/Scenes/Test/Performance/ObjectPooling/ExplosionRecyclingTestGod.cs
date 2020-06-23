using BattleCruisers.Effects.Explosions;
using BattleCruisers.Effects.Explosions.Pools;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;
using UnityEngine.Assertions;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Performance.ObjectPooling
{
    public class ExplosionRecyclingTestGod : TestGodBase
    {
        private IPool<IExplosion, Vector3> _pool;

        public float delayInS = 0.5f;
        public float xRadiusInM = 2.5f;
        public float yRadiusInM = 2.5f;

        protected override void Setup(Helper helper)
        {
            base.Setup(helper);

            IExplosionPoolChooser explosionPoolChooser = GetComponentInChildren<IExplosionPoolChooser>();
            Assert.IsNotNull(explosionPoolChooser);

            BuildableInitialisationArgs args = helper.CreateBuildableInitialisationArgs();
            _pool = explosionPoolChooser.ChoosePool(args.FactoryProvider.PoolProviders.ExplosionPoolProvider);

            InvokeRepeating(nameof(ShowExplosion), time: 0, repeatRate: delayInS);
        }

        private void ShowExplosion()
        {
            Vector2 spawnPosition = new Vector2(
                BCUtils.RandomGenerator.Instance.Range(-xRadiusInM, xRadiusInM),
                BCUtils.RandomGenerator.Instance.Range(-yRadiusInM, yRadiusInM));

            _pool.GetItem(spawnPosition);
        }
    }
}