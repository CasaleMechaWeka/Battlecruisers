using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{

    public class PvPBulletPoolChooser : MonoBehaviour, IPvPProjectilePoolChooser<PvPProjectileController, PvPProjectileActivationArgs<IProjectileStats>, IProjectileStats>
    {
        public bool highCalibre = false;
        public bool tinyBullet = false;
        public bool flakBullet = false;

        public IPool<PvPProjectileController, PvPProjectileActivationArgs<IProjectileStats>>
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