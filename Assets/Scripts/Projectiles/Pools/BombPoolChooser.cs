using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Projectiles.Pools
{
    public class BombPoolChooser : MonoBehaviour, IProjectilePoolChooser<BombController, ProjectileActivationArgs>
    {
        public bool stratBomb = false;

        public Pool<BombController, ProjectileActivationArgs>
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

