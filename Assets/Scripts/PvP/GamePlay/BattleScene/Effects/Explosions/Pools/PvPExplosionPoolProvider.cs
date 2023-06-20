using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions.Pools
{
    public class PvPExplosionPoolProvider : IPvPExplosionPoolProvider
    {
        public IPvPPool<IPvPExplosion, Vector3> BulletImpactPool { get; }
        public IPvPPool<IPvPExplosion, Vector3> HighCalibreBulletImpactPool { get; }
        public IPvPPool<IPvPExplosion, Vector3> TinyBulletImpactPool { get; }
        public IPvPPool<IPvPExplosion, Vector3> NovaShellImpactPool { get; }
        public IPvPPool<IPvPExplosion, Vector3> RocketShellImpactPool { get; }
        public IPvPPool<IPvPExplosion, Vector3> BombExplosionPool { get; }
        public IPvPPool<IPvPExplosion, Vector3> FlakExplosionsPool { get; }
        public IPvPPool<IPvPExplosion, Vector3> SmallExplosionsPool { get; }
        public IPvPPool<IPvPExplosion, Vector3> MediumExplosionsPool { get; }
        public IPvPPool<IPvPExplosion, Vector3> LargeExplosionsPool { get; }
        public IPvPPool<IPvPExplosion, Vector3> HugeExplosionsPool { get; }


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
            LargeExplosionsPool = CreateExplosionPool(prefabFactory, PvPStaticPrefabKeys.PvPExplosions.PvPExplosion150);
            HugeExplosionsPool = CreateExplosionPool(prefabFactory, PvPStaticPrefabKeys.PvPExplosions.PvPExplosion500);
        }

        private IPvPPool<IPvPExplosion, Vector3> CreateExplosionPool(IPvPPrefabFactory prefabFactory, PvPExplosionKey explosionKey)
        {
            return
                new PvPPool<IPvPExplosion, Vector3>(
                    new PvPExplosionFactory(
                        prefabFactory,
                        explosionKey));
        }

        public async Task SetInitialCapacity()
        {
            await TinyBulletImpactPool.AddCapacity(PvPInitialCapacity.BULLET_IMPACT);
            await HighCalibreBulletImpactPool.AddCapacity(PvPInitialCapacity.BULLET_IMPACT);
            await BulletImpactPool.AddCapacity(PvPInitialCapacity.BULLET_IMPACT);
            await BombExplosionPool.AddCapacity(PvPInitialCapacity.BOMB);
            await FlakExplosionsPool.AddCapacity(PvPInitialCapacity.FLAK);
            await SmallExplosionsPool.AddCapacity(PvPInitialCapacity.SMALL);
            await MediumExplosionsPool.AddCapacity(PvPInitialCapacity.MEDIUM);
            await LargeExplosionsPool.AddCapacity(PvPInitialCapacity.LARGE);
            await NovaShellImpactPool.AddCapacity(PvPInitialCapacity.LARGE);
            await RocketShellImpactPool.AddCapacity(PvPInitialCapacity.MEDIUM);
            await HugeExplosionsPool.AddCapacity(PvPInitialCapacity.HUGE);
        }
    }
}