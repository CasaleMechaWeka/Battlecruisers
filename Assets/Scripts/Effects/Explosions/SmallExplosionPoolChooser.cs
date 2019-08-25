using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Effects.Explosions
{
    public class SmallExplosionPoolChooser : MonoBehaviour, IExplosionPoolChooser
    {
        public IPool<IExplosion, Vector3> ChoosePool(IExplosionPoolProvider explosionPoolProvider)
        {
            return explosionPoolProvider.SmallExplosionsPool;
        }
    }
}