using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Deaths.Pools
{
    public interface IPvPShipDeathPoolProvider
    {
        IPvPPool<IPoolable<Vector3>, Vector3> AttackBoatPool { get; }
        IPvPPool<IPoolable<Vector3>, Vector3> AttackRIBPool { get; }
        IPvPPool<IPoolable<Vector3>, Vector3> FrigatePool { get; }
        IPvPPool<IPoolable<Vector3>, Vector3> DestroyerPool { get; }
        IPvPPool<IPoolable<Vector3>, Vector3> SiegeDestroyerPool { get; }
        IPvPPool<IPoolable<Vector3>, Vector3> ArchonPool { get; }
        IPvPPool<IPoolable<Vector3>, Vector3> GlassCannoneerPool { get; }
        IPvPPool<IPoolable<Vector3>, Vector3> GunBoatPool { get; }
        IPvPPool<IPoolable<Vector3>, Vector3> TurtlePool { get; }
    }
}