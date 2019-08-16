using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Projectiles.Pools
{
    public class RocketPoolChooser : MonoBehaviour, IProjectilePoolChooser<TargetProviderActivationArgs<ICruisingProjectileStats>, ICruisingProjectileStats>
    {
        public IPool<TargetProviderActivationArgs<ICruisingProjectileStats>> ChoosePool(IProjectilePoolProvider projectilePoolProvider)
        {
            return projectilePoolProvider.RocketsPool;
        }
    }
}