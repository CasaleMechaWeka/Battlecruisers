using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Projectiles.Pools
{
    public class MissilesSmallPoolChooser : MonoBehaviour, IProjectilePoolChooser<MissileController, TargetProviderActivationArgs<ProjectileStats>, ProjectileStats>
    {
        public Pool<MissileController, TargetProviderActivationArgs<ProjectileStats>> ChoosePool(IProjectilePoolProvider projectilePoolProvider)
        {
            return projectilePoolProvider.MissilesSmallPool;
        }
    }
}