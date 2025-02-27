using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Hotkeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Music;
using BattleCruisers.Utils.Threading;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Utils.BattleScene.Lifetime;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.UI;
using BattleCruisers.UI.Sound.Wind;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene
{
    public interface IPvPBattleSceneGodComponents
    {
        IAudioSource PrioritisedSoundPlayerAudioSource { get; }
        IAudioSource UISoundsAudioSource { get; }
        PvPLayeredMusicPlayerInitialiser MusicPlayerInitialiser { get; }
        IClickableEmitter BackgroundClickableEmitter { get; }
        IPvPTargetIndicator TargetIndicator { get; }
        WindInitialiser WindInitialiser { get; }
        PvPHotkeyInitialiser HotkeyInitialiser { get; }


        IDeferrer Deferrer { get; }
        IDeferrer RealTimeDeferrer { get; }

        IUpdaterProvider UpdaterProvider { get; }

        ILifetimeEventBroadcaster LifetimeEvents { get; }
    }
}