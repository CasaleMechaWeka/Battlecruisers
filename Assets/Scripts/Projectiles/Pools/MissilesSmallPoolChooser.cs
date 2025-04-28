using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Projectiles.Pools
{
    public class MissilesSmallPoolChooser : MonoBehaviour, IProjectilePoolChooser<MissileController, ProjectileActivationArgs>
    {
        public Pool<MissileController, ProjectileActivationArgs> ChoosePool(IProjectilePoolProvider projectilePoolProvider)
        {
            return projectilePoolProvider.MissilesSmallPool;
        }
    }
}