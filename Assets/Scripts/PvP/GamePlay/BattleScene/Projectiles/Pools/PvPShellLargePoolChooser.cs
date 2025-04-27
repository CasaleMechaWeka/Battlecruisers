using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{
    public class PvPShellLargePoolChooser : MonoBehaviour, IPvPProjectilePoolChooser<PvPProjectileController, ProjectileActivationArgs<ProjectileStats>, ProjectileStats>
    {

        public bool novaShell = false;
        public bool fiveShellCluster = false;

        public Pool<PvPProjectileController, ProjectileActivationArgs<ProjectileStats>>
            ChoosePool(IPvPProjectilePoolProvider projectilePoolProvider)
        {
            if (novaShell)
                return projectilePoolProvider.NovaShellPool;
            if (fiveShellCluster)
                return projectilePoolProvider.FiveShellCluster;

            return projectilePoolProvider.ShellsLargePool;
        }
    }
}
