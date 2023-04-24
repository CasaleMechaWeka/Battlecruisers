using BattleCruisers.Utils.Localisation;
using UnityEngine.Assertions;
using UnityEngine;


namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene
{
    public class PvPPrefab : MonoBehaviour, IPrefab
    {
        protected ILocTable _commonStrings;

        public virtual void StaticInitialise(ILocTable commonStrings)
        {
            Assert.IsNotNull(commonStrings);
            _commonStrings = commonStrings;
        }
    }
}

