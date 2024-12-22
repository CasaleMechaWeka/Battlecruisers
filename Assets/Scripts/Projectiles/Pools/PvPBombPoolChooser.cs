using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Pools;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Projectiles;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{
    public class PvPBombPoolChooser : MonoBehaviour, IPvPProjectilePoolChooser<PvPBombController, PvPProjectileActivationArgs<IPvPProjectileStats>, IPvPProjectileStats>
    {

        public bool stratBomb = false;


        public IPvPPool<PvPBombController, PvPProjectileActivationArgs<IPvPProjectileStats>>
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