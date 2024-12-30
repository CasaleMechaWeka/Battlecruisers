using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Hotkeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Music;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Wind;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Lifetime;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Update;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene
{
    public interface IPvPBattleSceneGodComponents
    {
        IAudioSource PrioritisedSoundPlayerAudioSource { get; }
        IAudioSource UISoundsAudioSource { get; }
        PvPLayeredMusicPlayerInitialiser MusicPlayerInitialiser { get; }
        IPvPClickableEmitter BackgroundClickableEmitter { get; }
        IPvPTargetIndicator TargetIndicator { get; }
        PvPWindInitialiser WindInitialiser { get; }
        PvPHotkeyInitialiser HotkeyInitialiser { get; }


        IDeferrer Deferrer { get; }
        IDeferrer RealTimeDeferrer { get; }

        IPvPUpdaterProvider UpdaterProvider { get; }

        IPvPLifetimeEventBroadcaster LifetimeEvents { get; }
    }
}