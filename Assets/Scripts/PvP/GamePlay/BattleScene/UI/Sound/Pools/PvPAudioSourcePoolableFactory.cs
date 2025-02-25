using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.UI.Sound.Pools;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Pools
{
    public class PvPAudioSourcePoolableFactory : IPvPAudioSourcePoolableFactory
    {
        private readonly IPvPPrefabFactory _prefabFactory;
        private readonly IDeferrer _realTimeDeferrer;

        public PvPAudioSourcePoolableFactory(IPvPPrefabFactory prefabFactory, IDeferrer realTimeDeferrer)
        {
            PvPHelper.AssertIsNotNull(prefabFactory, realTimeDeferrer);

            _prefabFactory = prefabFactory;
            _realTimeDeferrer = realTimeDeferrer;
        }

        public IPoolable<AudioSourceActivationArgs> CreateItem()
        {
            return _prefabFactory.CreateAudioSource(_realTimeDeferrer);
        }
    }
}