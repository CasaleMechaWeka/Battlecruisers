using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Projectiles.Pools
{
    public class ShellLargePoolChooser : MonoBehaviour, IProjectilePoolChooser<ProjectileController, ProjectileActivationArgs>
    {

        public bool novaShell = false;
        public bool FiveShellCluster = false;

        public Pool<ProjectileController, ProjectileActivationArgs>
            ChoosePool(IProjectilePoolProvider projectilePoolProvider)
        {
            if (novaShell)
                return projectilePoolProvider.NovaShellPool;
            if (FiveShellCluster)
                return projectilePoolProvider.FiveShellCluster;

            return projectilePoolProvider.ShellsLargePool;
        }
    }
}
