using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Projectiles.Pools
{
    public class MissilesSmartPoolChooser : MonoBehaviour, IProjectilePoolChooser<SmartMissileController, SmartMissileActivationArgs<SmartProjectileStats>, SmartProjectileStats>
    {
        public Pool<SmartMissileController, SmartMissileActivationArgs<SmartProjectileStats>> ChoosePool(IProjectilePoolProvider projectilePoolProvider)
        {
            return projectilePoolProvider.MissilesSmartPool;
        }
    }
}