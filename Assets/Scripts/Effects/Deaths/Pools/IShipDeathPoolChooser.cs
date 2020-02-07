using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Effects.Deaths.Pools
{
    public interface IShipDeathPoolChooser
    {
        IPool<IShipDeath, Vector3> ChoosePool(IShipDeathPoolProvider shipDeathPoolProvider);
    }
}