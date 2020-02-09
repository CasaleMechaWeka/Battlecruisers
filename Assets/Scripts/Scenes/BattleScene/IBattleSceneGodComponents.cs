using BattleCruisers.UI.Music;
using BattleCruisers.Utils.BattleScene.Lifetime;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Scenes.BattleScene
{
    public interface IBattleSceneGodComponents
    {
        IDeferrer Deferrer { get; }
        IAudioSource AudioSource { get; }
        ICamera Camera { get; }
        IUpdaterProvider UpdaterProvider { get; }
        LayeredMusicPlayerInitialiser MusicPlayerInitialiser { get; }
        ILifetimeEventBroadcaster LifetimeEvents { get; }
    }
}