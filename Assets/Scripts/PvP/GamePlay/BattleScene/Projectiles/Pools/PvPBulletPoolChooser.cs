using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{

    public class PvPBulletPoolChooser : MonoBehaviour, IPvPProjectilePoolChooser<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>, IPvPProjectileStats>
    {
        public bool highCalibre = false;
        public bool tinyBullet = false;

        public IPvPPool<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>>
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
            return projectilePoolProvider.BulletsPool;
        }
    }
}