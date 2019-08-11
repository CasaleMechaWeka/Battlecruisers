using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Effects.Explosions.Pools
{
    public class HugeSmallExplosionPoolChooser : MonoBehaviour, IExplosionPoolChooser
    {
        public IPool<Vector3> ChoosePool(IExplosionPoolProvider explosionPoolProvider)
        {
            return explosionPoolProvider.HugeExplosionsPool;
        }
    }
}