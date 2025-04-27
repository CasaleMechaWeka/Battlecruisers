using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Projectiles.Pools
{

    public class BulletPoolChooser : MonoBehaviour, IProjectilePoolChooser<ProjectileController, ProjectileActivationArgs<ProjectileStats>, ProjectileStats>
    {
        public bool highCalibre = false;
        public bool tinyBullet = false;
        public bool flakBullet = false;

        public Pool<ProjectileController, ProjectileActivationArgs<ProjectileStats>> ChoosePool(IProjectilePoolProvider projectilePoolProvider)
        {
            if (highCalibre)
            {
                return projectilePoolProvider.HighCalibreBulletsPool;
            }
            if (tinyBullet)
            {
                return projectilePoolProvider.TinyBulletsPool;
            }
            if (flakBullet)
            {
                return projectilePoolProvider.FlakBulletsPool;
            }
            return projectilePoolProvider.BulletsPool;
        }
    }
}