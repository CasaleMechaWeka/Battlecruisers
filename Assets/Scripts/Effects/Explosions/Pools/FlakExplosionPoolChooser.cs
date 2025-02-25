using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Effects.Explosions.Pools
{
    public class FlakExplosionPoolChooser : MonoBehaviour, IExplosionPoolChooser
    {
        public IPool<IPoolable<Vector3>, Vector3> ChoosePool(IExplosionPoolProvider explosionPoolProvider)
        {
            return explosionPoolProvider.FlakExplosionsPool;
        }
    }
}