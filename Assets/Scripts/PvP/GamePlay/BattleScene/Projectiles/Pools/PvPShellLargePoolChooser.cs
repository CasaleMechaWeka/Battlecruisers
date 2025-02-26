using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{
    public class PvPShellLargePoolChooser : MonoBehaviour, IPvPProjectilePoolChooser<PvPProjectileController, PvPProjectileActivationArgs<IProjectileStats>, IProjectileStats>
    {

        public bool novaShell = false;
        public bool fiveShellCluster = false;

        public IPool<PvPProjectileController, PvPProjectileActivationArgs<IProjectileStats>>
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
