using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Deaths.Pools
{
    public class PvPFrigateDeathPoolChooser : MonoBehaviour, IPvPShipDeathPoolChooser
    {
        public IPvPPool<IPvPShipDeath, Vector3> ChoosePool(IPvPShipDeathPoolProvider shipDeathPoolProvider)
        {
            return shipDeathPoolProvider.FrigatePool;
        }
    }
}