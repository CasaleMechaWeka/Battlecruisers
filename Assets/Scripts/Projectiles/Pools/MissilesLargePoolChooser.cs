using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Projectiles.Pools
{
    public class MissilesLargePoolChooser : MonoBehaviour, IProjectilePoolChooser<TargetProviderActivationArgs<IProjectileStats>, IProjectileStats>
    {
        public IPool<TargetProviderActivationArgs<IProjectileStats>> ChoosePool(IProjectilePoolProvider projectilePoolProvider)
        {
            return projectilePoolProvider.MissilesLargePool;
        }
    }
}