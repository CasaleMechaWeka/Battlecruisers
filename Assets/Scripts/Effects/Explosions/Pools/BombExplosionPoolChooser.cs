using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Effects.Explosions.Pools
{
    public class BombExplosionPoolChooser : MonoBehaviour, IExplosionPoolChooser
    {
        public Pool<IPoolable<Vector3>, Vector3> ChoosePool(IExplosionPoolProvider explosionPoolProvider)
        {
            return explosionPoolProvider.BombExplosionPool;
        }
    }
}