using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Effects.Deaths.Pools
{
    public interface IShipDeathPoolProvider
    {
        IPool<IPoolable<Vector3>, Vector3> AttackBoatPool { get; }
        IPool<IPoolable<Vector3>, Vector3> AttackRIBPool { get; }
        IPool<IPoolable<Vector3>, Vector3> FrigatePool { get; }
        IPool<IPoolable<Vector3>, Vector3> DestroyerPool { get; }
        IPool<IPoolable<Vector3>, Vector3> SiegeDestroyerPool { get; }
        IPool<IPoolable<Vector3>, Vector3> ArchonPool { get; }
        IPool<IPoolable<Vector3>, Vector3> GlassCannoneerPool { get; }
        IPool<IPoolable<Vector3>, Vector3> GunBoatPool { get; }
        IPool<IPoolable<Vector3>, Vector3> TurtlePool { get; }
    }
}