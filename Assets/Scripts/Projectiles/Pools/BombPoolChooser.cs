using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Pools;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Projectiles.Pools
{
    public class BombPoolChooser : MonoBehaviour, IProjectilePoolChooser<BombController, ProjectileActivationArgs<IProjectileStats>, IProjectileStats>
    {
        public bool stratBomb = false;

        public IPool<BombController, ProjectileActivationArgs<IProjectileStats>> 
            ChoosePool(IProjectilePoolProvider projectilePoolProvider)
        {

            if (stratBomb)
            {
                return projectilePoolProvider.StratBombsPool;
            }
            return projectilePoolProvider.BombsPool;
        }
    }
}

