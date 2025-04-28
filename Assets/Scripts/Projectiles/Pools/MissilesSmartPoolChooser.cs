using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Projectiles.Pools
{
    public class MissilesSmartPoolChooser : MonoBehaviour, IProjectilePoolChooser<SmartMissileController, ProjectileActivationArgs>
    {
        public Pool<SmartMissileController, ProjectileActivationArgs> ChoosePool(IProjectilePoolProvider projectilePoolProvider)
        {
            return projectilePoolProvider.MissilesSmartPool;
        }
    }
}