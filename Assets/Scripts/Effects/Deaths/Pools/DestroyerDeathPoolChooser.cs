using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Effects.Deaths.Pools
{
    public class DestroyerDeathPoolChooser : MonoBehaviour, IShipDeathPoolChooser
    {
        public IPool<IShipDeath, Vector3> ChoosePool(IShipDeathPoolProvider shipDeathPoolProvider)
        {
            return shipDeathPoolProvider.DestroyerPool;
        }
    }
}