using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Projectiles.Pools
{
    public class MissilesMediumPoolChooser : MonoBehaviour, IProjectilePoolChooser<MissileController, TargetProviderActivationArgs<IProjectileStats>, IProjectileStats>
    {
        
        public bool isRailSlug = false;

        public Pool<MissileController, TargetProviderActivationArgs<IProjectileStats>> ChoosePool(IProjectilePoolProvider projectilePoolProvider)
        {

                         if (isRailSlug)
            {
                return projectilePoolProvider.RailSlugsPool;
            }

            return projectilePoolProvider.MissilesMediumPool;
        }
    }
}