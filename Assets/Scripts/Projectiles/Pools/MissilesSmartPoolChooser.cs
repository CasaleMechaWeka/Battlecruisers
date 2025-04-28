using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Projectiles.Pools
{
    public class MissilesSmartPoolChooser : MonoBehaviour, IProjectilePoolChooser<SmartMissileController, SmartMissileActivationArgs<ProjectileStats>, ProjectileStats>
    {
        public Pool<SmartMissileController, SmartMissileActivationArgs<ProjectileStats>> ChoosePool(IProjectilePoolProvider projectilePoolProvider)
        {
            return projectilePoolProvider.MissilesSmartPool;
        }
    }
}