using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Deaths.Pools
{
    public interface IPvPShipDeathPoolProvider
    {
        IPvPPool<IPvPShipDeath, Vector3> AttackBoatPool { get; }
        IPvPPool<IPvPShipDeath, Vector3> AttackRIBPool { get; }
        IPvPPool<IPvPShipDeath, Vector3> FrigatePool { get; }
        IPvPPool<IPvPShipDeath, Vector3> DestroyerPool { get; }
        IPvPPool<IPvPShipDeath, Vector3> ArchonPool { get; }
    }
}