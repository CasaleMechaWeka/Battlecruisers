using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.UI.Sound.Pools
{
    public class AudioSourcePoolableFactory : IPoolableFactory<IPoolable<AudioSourceActivationArgs>, AudioSourceActivationArgs>
    {
        private readonly IDeferrer _realTimeDeferrer;

        public AudioSourcePoolableFactory(IDeferrer realTimeDeferrer)
        {
            Helper.AssertIsNotNull(realTimeDeferrer);

            _realTimeDeferrer = realTimeDeferrer;
        }

        public IPoolable<AudioSourceActivationArgs> CreateItem()
        {
            return PrefabFactory.CreateAudioSource(_realTimeDeferrer);
        }
    }
}