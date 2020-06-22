using BattleCruisers.UI;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.Sound.Wind;
using BattleCruisers.Utils.BattleScene.Lifetime;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Scenes.BattleScene
{
    public interface IBattleSceneGodComponents
    {
        IDeferrer Deferrer { get; }
        IDeferrer RealTimeDeferrer { get; }
        IAudioSource PrioritisedSoundPlayerAudioSource { get; }
        IAudioSource UISoundsAudioSource { get; }
        IUpdaterProvider UpdaterProvider { get; }
        LayeredMusicPlayerInitialiser MusicPlayerInitialiser { get; }
        ILifetimeEventBroadcaster LifetimeEvents { get; }
        IClickableEmitter BackgroundClickableEmitter { get; }
        ITargetIndicator TargetIndicator { get; }
        WindInitialiser WindInitialiser { get; }
    }
}