using BattleCruisers.Utils.BattleScene.Pools;

namespace BattleCruisers.UI.Sound.Pools
{
    public interface IAudioSourcePoolableFactory : IPoolableFactory<IAudioSourcePoolable, AudioSourceActivationArgs>
    {
    }
}