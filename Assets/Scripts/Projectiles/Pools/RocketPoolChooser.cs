using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Projectiles.Pools
{
    public class RocketPoolChooser : MonoBehaviour, IProjectilePoolChooser<TargetProviderActivationArgs<ICruisingProjectileStats>, ICruisingProjectileStats>
    {
        public IPool<ProjectileControllerBase<TargetProviderActivationArgs<ICruisingProjectileStats>, ICruisingProjectileStats>, TargetProviderActivationArgs<ICruisingProjectileStats>> 
            ChoosePool(IProjectilePoolProvider projectilePoolProvider)
        {
            return projectilePoolProvider.RocketsPool;
        }
    }
}