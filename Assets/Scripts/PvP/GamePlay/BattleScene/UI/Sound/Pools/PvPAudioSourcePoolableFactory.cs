using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.UI.Sound.Pools;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Pools
{
    public class PvPAudioSourcePoolableFactory : IPoolableFactory<IPoolable<AudioSourceActivationArgs>, AudioSourceActivationArgs>
    {
        private readonly PvPPrefabFactory _prefabFactory;
        private readonly IDeferrer _realTimeDeferrer;

        public PvPAudioSourcePoolableFactory(PvPPrefabFactory prefabFactory, IDeferrer realTimeDeferrer)
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