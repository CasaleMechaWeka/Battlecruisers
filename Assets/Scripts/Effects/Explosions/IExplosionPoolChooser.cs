using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Effects.Explosions
{
    public interface IExplosionPoolChooser
    {
        IPool<Vector3> ChoosePool(IExplosionPoolProvider explosionPoolProvider);
    }
}