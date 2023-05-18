using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Pools
{
    public class PvPAudioSourcePoolableFactory : IPvPAudioSourcePoolableFactory
    {
        private readonly IPvPPrefabFactory _prefabFactory;
        private readonly IPvPDeferrer _realTimeDeferrer;

        public PvPAudioSourcePoolableFactory(IPvPPrefabFactory prefabFactory, IPvPDeferrer realTimeDeferrer)
        {
            PvPHelper.AssertIsNotNull(prefabFactory, realTimeDeferrer);

            _prefabFactory = prefabFactory;
            _realTimeDeferrer = realTimeDeferrer;
        }

        public IPvPAudioSourcePoolable CreateItem()
        {
            return _prefabFactory.CreateAudioSource(_realTimeDeferrer);
        }
    }
}