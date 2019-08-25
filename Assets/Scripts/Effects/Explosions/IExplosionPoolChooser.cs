using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Effects.Explosions
{
    public interface IExplosionPoolChooser
    {
        IPool<IExplosion, Vector3> ChoosePool(IExplosionPoolProvider explosionPoolProvider);
    }
}