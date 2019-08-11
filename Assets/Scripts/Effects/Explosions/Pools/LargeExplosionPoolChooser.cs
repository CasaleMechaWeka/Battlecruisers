using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Effects.Explosions.Pools
{
    public class LargeExplosionPoolChooser : MonoBehaviour, IExplosionPoolChooser
    {
        public IPool<Vector3> ChoosePool(IExplosionPoolProvider explosionPoolProvider)
        {
            return explosionPoolProvider.LargeExplosionsPool;
        }
    }
}