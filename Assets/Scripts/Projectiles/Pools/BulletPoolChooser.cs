using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Projectiles.Pools
{
    
    public class BulletPoolChooser : MonoBehaviour, IProjectilePoolChooser<ProjectileController, ProjectileActivationArgs<IProjectileStats>, IProjectileStats>
    {
        public bool highCalibre = false;
        public bool tinyBullet = false;

        public IPool<ProjectileController, ProjectileActivationArgs<IProjectileStats>> 
            ChoosePool(IProjectilePoolProvider projectilePoolProvider)
        {
            if (highCalibre)
            {
                return projectilePoolProvider.HighCalibreBulletsPool;
            }
             if (tinyBullet)
            {
                return projectilePoolProvider.TinyBulletsPool;
            }
            return projectilePoolProvider.BulletsPool;
        }
    }
}