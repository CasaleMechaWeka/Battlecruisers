using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{
    public class PvPShellSmallPoolChooser : MonoBehaviour, IPvPProjectilePoolChooser<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>, IPvPProjectileStats>
    {
        public bool rocketShell = false;

        public IPvPPool<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>>
            ChoosePool(IPvPProjectilePoolProvider projectilePoolProvider)
        {
            if (rocketShell)
            {
                return projectilePoolProvider.RocketShellPool;
            }

            return projectilePoolProvider.ShellsSmallPool;
        }
    }
}

