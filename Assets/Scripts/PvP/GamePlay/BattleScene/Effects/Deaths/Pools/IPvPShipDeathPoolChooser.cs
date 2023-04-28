using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Deaths.Pools
{
    public interface IPvPShipDeathPoolChooser
    {
        IPvPPool<IPvPShipDeath, Vector3> ChoosePool(IPvPShipDeathPoolProvider shipDeathPoolProvider);
    }
}