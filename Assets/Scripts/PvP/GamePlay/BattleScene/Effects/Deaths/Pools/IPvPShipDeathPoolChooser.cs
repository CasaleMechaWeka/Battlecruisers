using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Deaths.Pools
{
    public interface IPvPShipDeathPoolChooser
    {
        IPvPPool<IPoolable<Vector3>, Vector3> ChoosePool(IPvPShipDeathPoolProvider shipDeathPoolProvider);
    }
}