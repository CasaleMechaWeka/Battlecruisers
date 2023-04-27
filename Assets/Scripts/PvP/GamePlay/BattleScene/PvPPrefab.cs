using BattleCruisers.Utils.Localisation;
using UnityEngine.Assertions;
using UnityEngine;
using Unity.Netcode;


namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene
{
    public class PvPPrefab : NetworkBehaviour, IPrefab
    {
        protected ILocTable _commonStrings;

        public virtual void StaticInitialise(ILocTable commonStrings)
        {
            Assert.IsNotNull(commonStrings);
            _commonStrings = commonStrings;
        }
    }
}

