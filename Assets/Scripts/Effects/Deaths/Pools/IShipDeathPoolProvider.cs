using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Effects.Deaths.Pools
{
    public interface IShipDeathPoolProvider
    {
        IPool<IShipDeath, Vector3> AttackBoatPool { get; }
        IPool<IShipDeath, Vector3> AttackRIBPool { get; }
        IPool<IShipDeath, Vector3> FrigatePool { get; }
        IPool<IShipDeath, Vector3> DestroyerPool { get; }
        IPool<IShipDeath, Vector3> ArchonPool { get; }
    }
}