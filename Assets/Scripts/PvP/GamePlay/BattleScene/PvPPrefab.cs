using BattleCruisers.Utils.Localisation;
using UnityEngine.Assertions;
using UnityEngine;
using Unity.Netcode;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene
{
    public class PvPPrefab : NetworkBehaviour, IPvPPrefab
    {
        protected ILocTable _commonStrings;


        public virtual void StaticInitialise(ILocTable commonStrings)
        {
            Assert.IsNotNull(commonStrings);
            _commonStrings = commonStrings;
        }
    }
}

