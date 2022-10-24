using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Projectiles.Pools
{
    public class ShellSmallPoolChooser : MonoBehaviour, IProjectilePoolChooser<ProjectileController, ProjectileActivationArgs<IProjectileStats>, IProjectileStats>
    {
        public bool rocketShell = false;

        public IPool<ProjectileController, ProjectileActivationArgs<IProjectileStats>> 
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

