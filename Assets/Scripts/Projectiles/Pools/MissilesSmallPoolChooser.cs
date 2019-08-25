using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Projectiles.Pools
{
    public class MissilesSmallPoolChooser : MonoBehaviour, IProjectilePoolChooser<TargetProviderActivationArgs<IProjectileStats>, IProjectileStats>
    {
        public IPool<ProjectileControllerBase<TargetProviderActivationArgs<IProjectileStats>, IProjectileStats>, TargetProviderActivationArgs<IProjectileStats>>
            ChoosePool(IProjectilePoolProvider projectilePoolProvider)
        {
            return projectilePoolProvider.MissilesSmallPool;
        }
    }
}