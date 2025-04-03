using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.UI.Sound.Pools;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Pools
{
    public class PvPAudioSourcePoolableFactory : IPoolableFactory<IPoolable<AudioSourceActivationArgs>, AudioSourceActivationArgs>
    {
        private readonly IDeferrer _realTimeDeferrer;

        public PvPAudioSourcePoolableFactory(IDeferrer realTimeDeferrer)
        {
            PvPHelper.AssertIsNotNull(realTimeDeferrer);

            _realTimeDeferrer = realTimeDeferrer;
        }

        public IPoolable<AudioSourceActivationArgs> CreateItem()
        {
            return PvPPrefabFactory.CreateAudioSource(_realTimeDeferrer);
        }
    }
}