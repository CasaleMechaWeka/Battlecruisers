using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Hotkeys;
using BattleCruisers.UI;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.Sound.Wind;
using BattleCruisers.Utils.Threading;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Utils.BattleScene.Lifetime;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.UI.Music;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene
{
    public interface IPvPBattleSceneGodComponents
    {
        IAudioSource PrioritisedSoundPlayerAudioSource { get; }
        IAudioSource UISoundsAudioSource { get; }
        LayeredMusicPlayerInitialiser MusicPlayerInitialiser { get; }
        IClickableEmitter BackgroundClickableEmitter { get; }
        ITargetIndicator TargetIndicator { get; }
        WindInitialiser WindInitialiser { get; }
        PvPHotkeyInitialiser HotkeyInitialiser { get; }


        IDeferrer Deferrer { get; }
        IDeferrer RealTimeDeferrer { get; }

        IUpdaterProvider UpdaterProvider { get; }

        LifetimeEventBroadcaster LifetimeEvents { get; }
    }
}