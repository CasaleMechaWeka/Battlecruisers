using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Effects.Explosions.Pools
{
    public interface IExplosionPoolChooser
    {
        IPool<IExplosion, Vector3> ChoosePool(IExplosionPoolProvider explosionPoolProvider);
    }
}