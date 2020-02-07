using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Effects.Deaths.Pools
{
    public interface IShipDeathPoolProvider
    {
        // FELIX  Add other ships :)
        IPool<IShipDeath, Vector3> ArchonPool { get; }
    }
}