using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Effects.Deaths.Pools
{
    public class ArchonDeathPoolChooser : MonoBehaviour, IShipDeathPoolChooser
    {
        public Pool<IPoolable<Vector3>, Vector3> ChoosePool(IShipDeathPoolProvider shipDeathPoolProvider)
        {
            return shipDeathPoolProvider.ArchonPool;
        }
    }
}