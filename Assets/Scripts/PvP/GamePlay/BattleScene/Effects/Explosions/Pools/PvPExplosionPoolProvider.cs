using BattleCruisers.Effects.Explosions.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions.Pools
{
    public class PvPExplosionPoolProvider : IExplosionPoolProvider
    {
        public IPool<IPoolable<Vector3>, Vector3> BulletImpactPool { get; }
        public IPool<IPoolable<Vector3>, Vector3> HighCalibreBulletImpactPool { get; }
        public IPool<IPoolable<Vector3>, Vector3> TinyBulletImpactPool { get; }
        public IPool<IPoolable<Vector3>, Vector3> NovaShellImpactPool { get; }
        public IPool<IPoolable<Vector3>, Vector3> RocketShellImpactPool { get; }
        public IPool<IPoolable<Vector3>, Vector3> BombExplosionPool { get; }
        public IPool<IPoolable<Vector3>, Vector3> FlakExplosionsPool { get; }
        public IPool<IPoolable<Vector3>, Vector3> SmallExplosionsPool { get; }
        public IPool<IPoolable<Vector3>, Vector3> MediumExplosionsPool { get; }
        public IPool<IPoolable<Vector3>, Vector3> MFExplosionsPool { get; }
        public IPool<IPoolable<Vector3>, Vector3> FirecrackerExplosionsPool { get; }
        public IPool<IPoolable<Vector3>, Vector3> LargeExplosionsPool { get; }
        public IPool<IPoolable<Vector3>, Vector3> HugeExplosionsPool { get; }
        public IPool<IPoolable<Vector3>, Vector3> FiveShellClusterExplosionsPool { get; }

        public PvPExplosionPoolProvider(IPvPPrefabFactory prefabFactory)
        {
            Assert.IsNotNull(prefabFactory);

            BulletImpactPool = CreateExplosionPool(prefabFactory, PvPStaticPrefabKeys.PvPExplosions.PvPBulletImpact);
            HighCalibreBulletImpactPool = CreateExplosionPool(prefabFactory, PvPStaticPrefabKeys.PvPExplosions.PvPHighCalibreBulletImpact);
            TinyBulletImpactPool = CreateExplosionPool(prefabFactory, PvPStaticPrefabKeys.PvPExplosions.PvPTinyBulletImpact);
            NovaShellImpactPool = CreateExplosionPool(prefabFactory, PvPStaticPrefabKeys.PvPExplosions.PvPNovaShellImpact);
            RocketShellImpactPool = CreateExplosionPool(prefabFactory, PvPStaticPrefabKeys.PvPExplosions.PvPRocketShellImpact);
            BombExplosionPool = CreateExplosionPool(prefabFactory, PvPStaticPrefabKeys.PvPExplosions.PvPBombExplosion);
            FlakExplosionsPool = CreateExplosionPool(prefabFactory, PvPStaticPrefabKeys.PvPExplosions.PvPFlakExplosion);
            SmallExplosionsPool = CreateExplosionPool(prefabFactory, PvPStaticPrefabKeys.PvPExplosions.PvPExplosion75);
            MediumExplosionsPool = CreateExplosionPool(prefabFactory, PvPStaticPrefabKeys.PvPExplosions.PvPExplosion100);
            MFExplosionsPool = CreateExplosionPool(prefabFactory, PvPStaticPrefabKeys.PvPExplosions.PvPExplosionMF);
            FirecrackerExplosionsPool = CreateExplosionPool(prefabFactory, PvPStaticPrefabKeys.PvPExplosions.PvPExplosionFirecracker);
            LargeExplosionsPool = CreateExplosionPool(prefabFactory, PvPStaticPrefabKeys.PvPExplosions.PvPExplosion150);
            HugeExplosionsPool = CreateExplosionPool(prefabFactory, PvPStaticPrefabKeys.PvPExplosions.PvPExplosion500);
            FiveShellClusterExplosionsPool = CreateExplosionPool(prefabFactory, PvPStaticPrefabKeys.PvPExplosions.PvPExplosionFiveShellCluster);
        }

        private IPool<IPoolable<Vector3>, Vector3> CreateExplosionPool(IPvPPrefabFactory prefabFactory, PvPExplosionKey explosionKey)
        {
            return
                new PvPPool<IPoolable<Vector3>, Vector3>(
                    new PvPExplosionFactory(
                        prefabFactory,
                        explosionKey));
        }

        public void SetInitialCapacity()
        {
            TinyBulletImpactPool.AddCapacity(0);
            HighCalibreBulletImpactPool.AddCapacity(0);
            BulletImpactPool.AddCapacity(0);
            BombExplosionPool.AddCapacity(0);
            FlakExplosionsPool.AddCapacity(0);
            SmallExplosionsPool.AddCapacity(0);
            MediumExplosionsPool.AddCapacity(0);
            MFExplosionsPool.AddCapacity(0);
            FirecrackerExplosionsPool.AddCapacity(0);
            LargeExplosionsPool.AddCapacity(0);
            NovaShellImpactPool.AddCapacity(0);
            RocketShellImpactPool.AddCapacity(0);
            HugeExplosionsPool.AddCapacity(0);
            FiveShellClusterExplosionsPool.AddCapacity(0);
        }

        public void SetInitialCapacity_Rest()
        {
            TinyBulletImpactPool.AddCapacity(PvPInitialCapacity.BULLET_IMPACT - 1);
            HighCalibreBulletImpactPool.AddCapacity(PvPInitialCapacity.BULLET_IMPACT - 1);
            BulletImpactPool.AddCapacity(PvPInitialCapacity.BULLET_IMPACT - 1);
            BombExplosionPool.AddCapacity(PvPInitialCapacity.BOMB - 1);
            FlakExplosionsPool.AddCapacity(PvPInitialCapacity.FLAK - 1);
            SmallExplosionsPool.AddCapacity(PvPInitialCapacity.SMALL - 1);
            MediumExplosionsPool.AddCapacity(PvPInitialCapacity.MEDIUM - 1);
            MFExplosionsPool.AddCapacity(PvPInitialCapacity.MEDIUM - 1);
            FirecrackerExplosionsPool.AddCapacity(PvPInitialCapacity.MEDIUM - 1);
            LargeExplosionsPool.AddCapacity(PvPInitialCapacity.LARGE - 1);
            NovaShellImpactPool.AddCapacity(PvPInitialCapacity.LARGE - 1);
            RocketShellImpactPool.AddCapacity(PvPInitialCapacity.MEDIUM - 1);
            HugeExplosionsPool.AddCapacity(PvPInitialCapacity.HUGE - 1);
            FiveShellClusterExplosionsPool.AddCapacity(PvPInitialCapacity.MEDIUM - 1);
        }
    }
}