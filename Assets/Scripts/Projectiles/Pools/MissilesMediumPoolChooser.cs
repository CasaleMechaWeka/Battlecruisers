using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Projectiles.Pools
{
    public class MissilesMediumPoolChooser : MonoBehaviour, IProjectilePoolChooser<MissileController, ProjectileActivationArgs>
    {

        public bool isRailSlug = false;

        public Pool<MissileController, ProjectileActivationArgs> ChoosePool(IProjectilePoolProvider projectilePoolProvider)
        {

            if (isRailSlug)
            {
                return projectilePoolProvider.RailSlugsPool;
            }

            return projectilePoolProvider.MissilesMediumPool;
        }
    }
}