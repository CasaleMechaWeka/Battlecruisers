using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{

    public class PvPBulletPoolChooser : MonoBehaviour, IPvPProjectilePoolChooser<PvPProjectileController, ProjectileActivationArgs<ProjectileStats>, ProjectileStats>
    {
        public bool highCalibre = false;
        public bool tinyBullet = false;
        public bool flakBullet = false;

        public Pool<PvPProjectileController, ProjectileActivationArgs<ProjectileStats>>
            ChoosePool(IPvPProjectilePoolProvider projectilePoolProvider)
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