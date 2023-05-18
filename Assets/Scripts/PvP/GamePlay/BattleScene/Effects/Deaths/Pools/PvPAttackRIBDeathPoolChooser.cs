using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Deaths.Pools
{
    public class PvPAttackRIBDeathPoolChooser : MonoBehaviour, IPvPShipDeathPoolChooser
    {
        public IPvPPool<IPvPShipDeath, Vector3> ChoosePool(IPvPShipDeathPoolProvider shipDeathPoolProvider)
        {
            return shipDeathPoolProvider.AttackRIBPool;
        }
    }
}