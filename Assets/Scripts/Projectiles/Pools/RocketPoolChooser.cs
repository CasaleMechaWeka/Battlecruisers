using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Projectiles.Pools
{
    public class RocketPoolChooser : MonoBehaviour, IProjectilePoolChooser<RocketController, ProjectileActivationArgs>
    {
        public bool isSmall = false;

        public Pool<RocketController, ProjectileActivationArgs>
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