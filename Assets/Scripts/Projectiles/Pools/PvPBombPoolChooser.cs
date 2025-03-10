using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{
    public class PvPBombPoolChooser : MonoBehaviour, IPvPProjectilePoolChooser<PvPBombController, PvPProjectileActivationArgs<IProjectileStats>, IProjectileStats>
    {

        public bool stratBomb = false;


        public IPool<PvPBombController, PvPProjectileActivationArgs<IProjectileStats>>
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