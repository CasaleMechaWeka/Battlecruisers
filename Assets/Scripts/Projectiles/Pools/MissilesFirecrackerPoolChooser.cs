using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Projectiles.Pools
{
    public class MissilesFirecrackerPoolChooser : MonoBehaviour, IProjectilePoolChooser<RocketController, ProjectileActivationArgs>
    {
        public Pool<RocketController, ProjectileActivationArgs> ChoosePool(IProjectilePoolProvider projectilePoolProvider)
        {
            return projectilePoolProvider.MissilesFirecrackerPool;
        }
    }
}