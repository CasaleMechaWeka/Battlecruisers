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
        IPool<IShipDeath, Vector3> SiegeDestroyerPool { get; }
        IPool<IShipDeath, Vector3> ArchonPool { get; }
        IPool<IShipDeath, Vector3> GlassCannoneerPool { get; }
        IPool<IShipDeath, Vector3> GunBoatPool { get; }
        IPool<IShipDeath, Vector3> RocketTurtlePool { get; }
    }
}