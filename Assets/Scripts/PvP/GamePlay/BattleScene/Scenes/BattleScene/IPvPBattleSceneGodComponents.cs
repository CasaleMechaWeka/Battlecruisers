using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Hotkeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Music;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Wind;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Lifetime;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Update;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene
{
    public interface IPvPBattleSceneGodComponents
    {
        IPvPDeferrer Deferrer { get; }
        IPvPDeferrer RealTimeDeferrer { get; }
        IPvPAudioSource PrioritisedSoundPlayerAudioSource { get; }
        IPvPAudioSource UISoundsAudioSource { get; }
        IPvPUpdaterProvider UpdaterProvider { get; }
        PvPLayeredMusicPlayerInitialiser MusicPlayerInitialiser { get; }
        IPvPLifetimeEventBroadcaster LifetimeEvents { get; }
        IPvPClickableEmitter BackgroundClickableEmitter { get; }
        IPvPTargetIndicator TargetIndicator { get; }
        PvPWindInitialiser WindInitialiser { get; }
        PvPHotkeyInitialiser HotkeyInitialiser { get; }
    }
}