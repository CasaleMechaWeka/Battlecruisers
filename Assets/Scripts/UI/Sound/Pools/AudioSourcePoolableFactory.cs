using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.UI.Sound.Pools
{
    public class AudioSourcePoolableFactory : IPoolableFactory<IPoolable<AudioSourceActivationArgs>, AudioSourceActivationArgs>
    {
        private readonly IPrefabFactory _prefabFactory;
        private readonly IDeferrer _realTimeDeferrer;

        public AudioSourcePoolableFactory(IPrefabFactory prefabFactory, IDeferrer realTimeDeferrer)
        {
            Helper.AssertIsNotNull(prefabFactory, realTimeDeferrer);

            _prefabFactory = prefabFactory;
            _realTimeDeferrer = realTimeDeferrer;
        }

        public IPoolable<AudioSourceActivationArgs> CreateItem()
        {
            return _prefabFactory.CreateAudioSource(_realTimeDeferrer);
        }
    }
}