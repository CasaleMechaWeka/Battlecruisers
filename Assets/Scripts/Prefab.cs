using BattleCruisers.Utils.Localisation;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers
{
    public class Prefab : MonoBehaviour, IPrefab
    {
        protected ILocTable _commonStrings;

        public virtual void StaticInitialise(ILocTable commonStrings) 
        {
            Assert.IsNotNull(commonStrings);
            _commonStrings = commonStrings;
        }
    }
}