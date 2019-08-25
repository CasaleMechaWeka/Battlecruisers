using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Projectiles.Pools
{
    public class RocketPoolChooser : MonoBehaviour, IProjectilePoolChooser<RocketController, TargetProviderActivationArgs<ICruisingProjectileStats>, ICruisingProjectileStats>
    {
        public IPool<RocketController, TargetProviderActivationArgs<ICruisingProjectileStats>> 
            ChoosePool(IProjectilePoolProvider projectilePoolProvider)
        {
            return projectilePoolProvider.RocketsPool;
        }
    }
}