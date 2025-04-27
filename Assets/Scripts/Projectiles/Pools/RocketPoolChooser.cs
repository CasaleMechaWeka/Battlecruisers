using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Projectiles.Pools
{
    public class RocketPoolChooser : MonoBehaviour, IProjectilePoolChooser<RocketController, TargetProviderActivationArgs<CruisingProjectileStats>, CruisingProjectileStats>
    {
        public bool isSmall = false;

        public Pool<RocketController, TargetProviderActivationArgs<CruisingProjectileStats>>
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