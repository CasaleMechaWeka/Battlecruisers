using BattleCruisers.Effects.Deaths.Pools;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Deaths.Pools
{
    public class PvPArchonDeathPoolChooser : MonoBehaviour, IShipDeathPoolChooser
    {
        public IPool<IPoolable<Vector3>, Vector3> ChoosePool(IShipDeathPoolProvider shipDeathPoolProvider)
        {
            return shipDeathPoolProvider.ArchonPool;
        }
    }
}