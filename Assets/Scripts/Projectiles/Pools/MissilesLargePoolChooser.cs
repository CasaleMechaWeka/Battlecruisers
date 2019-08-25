using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Projectiles.Pools
{
    public class MissilesLargePoolChooser : MonoBehaviour, IProjectilePoolChooser<MissileController, TargetProviderActivationArgs<IProjectileStats>, IProjectileStats>
    {
        public IPool<MissileController, TargetProviderActivationArgs<IProjectileStats>> ChoosePool(IProjectilePoolProvider projectilePoolProvider)
        {
            return projectilePoolProvider.MissilesLargePool;
        }
    }
}