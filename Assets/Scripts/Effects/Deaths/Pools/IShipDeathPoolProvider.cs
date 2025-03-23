using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Effects.Deaths.Pools
{
    public interface IShipDeathPoolProvider
    {
        Pool<IPoolable<Vector3>, Vector3> AttackBoatPool { get; }
        Pool<IPoolable<Vector3>, Vector3> AttackRIBPool { get; }
        Pool<IPoolable<Vector3>, Vector3> FrigatePool { get; }
        Pool<IPoolable<Vector3>, Vector3> DestroyerPool { get; }
        Pool<IPoolable<Vector3>, Vector3> SiegeDestroyerPool { get; }
        Pool<IPoolable<Vector3>, Vector3> ArchonPool { get; }
        Pool<IPoolable<Vector3>, Vector3> GlassCannoneerPool { get; }
        Pool<IPoolable<Vector3>, Vector3> GunBoatPool { get; }
        Pool<IPoolable<Vector3>, Vector3> TurtlePool { get; }
    }
}