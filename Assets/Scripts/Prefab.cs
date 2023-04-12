using BattleCruisers.Utils.Localisation;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Netcode;

namespace BattleCruisers
{
    public class Prefab : NetworkBehaviour, IPrefab
    {
        protected ILocTable _commonStrings;

        public virtual void StaticInitialise(ILocTable commonStrings)
        {
            Assert.IsNotNull(commonStrings);
            _commonStrings = commonStrings;
        }
    }



}