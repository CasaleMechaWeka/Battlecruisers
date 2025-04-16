using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.UI.Sound.Pools
{
    public class AudioSourcePoolableFactory : IPoolableFactory<IPoolable<AudioSourceActivationArgs>, AudioSourceActivationArgs>
    {
        public IPoolable<AudioSourceActivationArgs> CreateItem()
        {
            return PrefabFactory.CreateAudioSource();
        }
    }
}