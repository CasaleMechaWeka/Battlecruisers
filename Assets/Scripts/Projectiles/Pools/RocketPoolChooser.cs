using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Projectiles.Pools
{
    public class RocketPoolChooser : MonoBehaviour, IProjectilePoolChooser<RocketController, TargetProviderActivationArgs<ProjectileStats>, ProjectileStats>
    {
        public bool isSmall = false;

        public Pool<RocketController, TargetProviderActivationArgs<ProjectileStats>>
            ChoosePool(IProjectilePoolProvider projectilePoolProvider)
        {
            if (isSmall)
            {
                return projectilePoolProvider.RocketsSmallPool;
            }

            return projectilePoolProvider.RocketsPool;
        }
    }
}