using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Projectiles.Pools
{
    public class RocketPoolChooser : MonoBehaviour, IProjectilePoolChooser<RocketController, TargetProviderActivationArgs<ICruisingProjectileStats>, ICruisingProjectileStats>
    {
        public bool isSmall = false;

        public IPool<RocketController, TargetProviderActivationArgs<ICruisingProjectileStats>> 
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