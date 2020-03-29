using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Projectiles.Pools
{
    public class BombPoolChooser : MonoBehaviour, IProjectilePoolChooser<BombController, ProjectileActivationArgs<IProjectileStats>, IProjectileStats>
    {
        public IPool<BombController, ProjectileActivationArgs<IProjectileStats>> 
            ChoosePool(IProjectilePoolProvider projectilePoolProvider)
        {
            return projectilePoolProvider.BombsPool;
        }
    }
}