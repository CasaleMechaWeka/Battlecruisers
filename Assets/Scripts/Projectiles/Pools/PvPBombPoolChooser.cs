using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{
    public class PvPBombPoolChooser : MonoBehaviour, IPvPProjectilePoolChooser<PvPBombController, ProjectileActivationArgs, ProjectileStats>
    {

        public bool stratBomb = false;


        public Pool<PvPBombController, ProjectileActivationArgs>
            ChoosePool(IPvPProjectilePoolProvider projectilePoolProvider)
        {
            if (stratBomb)
            {
                return projectilePoolProvider.StratBombsPool;
            }
            return projectilePoolProvider.BombsPool;
        }


    }
}