using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{
    public class PvPBombPoolChooser : MonoBehaviour, IPvPProjectilePoolChooser<PvPBombController, ProjectileActivationArgs<IProjectileStats>, IProjectileStats>
    {

        public bool stratBomb = false;


        public IPool<PvPBombController, ProjectileActivationArgs<IProjectileStats>>
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