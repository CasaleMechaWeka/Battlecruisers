using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Projectiles.Pools
{
    public class MissilesLargePoolChooser : MonoBehaviour, IProjectilePoolChooser<MissileController, ProjectileActivationArgs>
    {
        public Pool<MissileController, ProjectileActivationArgs> ChoosePool(IProjectilePoolProvider projectilePoolProvider)
        {
            return projectilePoolProvider.MissilesLargePool;
        }
    }
}