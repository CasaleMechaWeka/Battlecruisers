using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Projectiles.Pools
{
    public class BulletPoolChooser : MonoBehaviour, IProjectilePoolChooser<ProjectileActivationArgs<IProjectileStats>, IProjectileStats>
    {
        public IPool<ProjectileControllerBase<ProjectileActivationArgs<IProjectileStats>, IProjectileStats>, ProjectileActivationArgs<IProjectileStats>> 
            ChoosePool(IProjectilePoolProvider projectilePoolProvider)
        {
            return projectilePoolProvider.BulletsPool;
        }
    }
}