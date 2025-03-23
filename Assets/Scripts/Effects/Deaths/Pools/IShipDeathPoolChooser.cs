using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Effects.Deaths.Pools
{
    public interface IShipDeathPoolChooser
    {
        Pool<IPoolable<Vector3>, Vector3> ChoosePool(IShipDeathPoolProvider shipDeathPoolProvider);
    }
}