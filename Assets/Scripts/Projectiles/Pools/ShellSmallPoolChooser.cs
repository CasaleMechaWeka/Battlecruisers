using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Projectiles.Pools
{
    public class ShellSmallPoolChooser : MonoBehaviour, IProjectilePoolChooser<ProjectileController, ProjectileActivationArgs>
    {
        public bool rocketShell = false;

        public Pool<ProjectileController, ProjectileActivationArgs>
            ChoosePool(IProjectilePoolProvider projectilePoolProvider)
        {
            if (rocketShell)
            {
                return projectilePoolProvider.RocketShellPool;
            }

            return projectilePoolProvider.ShellsSmallPool;
        }
    }
}

