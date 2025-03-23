using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Projectiles.Pools
{
    public class MissilesFirecrackerPoolChooser : MonoBehaviour, IProjectilePoolChooser<RocketController, TargetProviderActivationArgs<ICruisingProjectileStats>, ICruisingProjectileStats>
    {
        public Pool<RocketController, TargetProviderActivationArgs<ICruisingProjectileStats>> ChoosePool(IProjectilePoolProvider projectilePoolProvider)
        {
            return projectilePoolProvider.MissilesFirecrackerPool;
        }
    }
}