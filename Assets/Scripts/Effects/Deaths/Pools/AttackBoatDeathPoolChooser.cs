using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Effects.Deaths.Pools
{
    public class AttackBoatDeathPoolChooser : MonoBehaviour, IShipDeathPoolChooser
    {
        public IPool<IPoolable<Vector3>, Vector3> ChoosePool(IShipDeathPoolProvider shipDeathPoolProvider)
        {
            return shipDeathPoolProvider.AttackBoatPool;
        }
    }
}