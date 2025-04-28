using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{
    public class PvPShellSmallPoolChooser : MonoBehaviour, IPvPProjectilePoolChooser<PvPProjectileController, ProjectileActivationArgs, ProjectileStats>
    {
        public bool rocketShell = false;

        public Pool<PvPProjectileController, ProjectileActivationArgs>
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

