using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Effects.Explosions.Pools
{
    public class NovaShellImpactPoolChooser : MonoBehaviour, IExplosionPoolChooser
    {
        public Pool<IPoolable<Vector3>, Vector3> ChoosePool(IExplosionPoolProvider explosionPoolProvider)
        {
            return explosionPoolProvider.NovaShellImpactPool;
        }
    }
}