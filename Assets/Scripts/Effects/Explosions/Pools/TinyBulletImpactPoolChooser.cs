using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Effects.Explosions.Pools
{
    public class TinyBulletImpactPoolChooser : MonoBehaviour, IExplosionPoolChooser
    {
        public IPool<IExplosion, Vector3> ChoosePool(IExplosionPoolProvider explosionPoolProvider)
        {
            return explosionPoolProvider.TinyBulletImpactPool;
        }
    }
}