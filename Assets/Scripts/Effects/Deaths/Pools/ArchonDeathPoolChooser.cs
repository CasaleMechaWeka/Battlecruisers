using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Effects.Deaths.Pools
{
    public class ArchonDeathPoolChooser : MonoBehaviour, IArchonDeathPoolChooser
    {
        public IPool<IShipDeath, Vector3> ChoosePool(IShipDeathPoolProvider shipDeathPoolProvider)
        {
            return shipDeathPoolProvider.ArchonPool;
        }
    }
}