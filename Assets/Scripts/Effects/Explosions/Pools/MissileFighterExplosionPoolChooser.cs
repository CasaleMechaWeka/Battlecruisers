using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Projectiles.Pools
{
    public class MissileFighterExplosionPoolChooser : MonoBehaviour, IProjectilePoolChooser<SmartMissileController, SmartMissileActivationArgs<ISmartProjectileStats>, ISmartProjectileStats>
    {
        public IPool<SmartMissileController, SmartMissileActivationArgs<ISmartProjectileStats>> ChoosePool(IProjectilePoolProvider projectilePoolProvider)
        {
            return projectilePoolProvider.MissileFighterSmartPool;
        }
    }
}