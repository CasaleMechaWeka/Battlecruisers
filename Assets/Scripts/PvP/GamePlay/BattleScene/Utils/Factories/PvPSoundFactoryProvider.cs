using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.ProjectileSpawners;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories
{
    public class PvPSoundFactoryProvider : IPvPSoundFactoryProvider
    {
        public IPvPSoundFetcher SoundFetcher { get; }
        public IPvPSoundPlayer SoundPlayer { get; }
        public IPvPPrioritisedSoundPlayer PrioritisedSoundPlayer { get; }
        public IPvPPrioritisedSoundPlayer DummySoundPlayer { get; }
        public IPvPSingleSoundPlayer UISoundPlayer { get; }
        public IPvPSoundPlayerFactory SoundPlayerFactory { get; }

        public PvPSoundFactoryProvider(IPvPBattleSceneGodComponents components, IPvPPoolProviders poolProviders)
        {
            PvPHelper.AssertIsNotNull(components, poolProviders);

            SoundFetcher = new PvPSoundFetcher();
            SoundPlayer = new PvPSoundPlayer(SoundFetcher, poolProviders.AudioSourcePool);
            UISoundPlayer = new PvPSingleSoundPlayer(SoundFetcher, components.UISoundsAudioSource);
            SoundPlayerFactory = new PvPSoundPlayerFactory(SoundFetcher, components.Deferrer);
            DummySoundPlayer = new PvPDummySoundPlayer();

            PrioritisedSoundPlayer
                = new PvPPrioritisedSoundPlayer(
                    new PvPSingleSoundPlayer(
                        SoundFetcher,
                        components.PrioritisedSoundPlayerAudioSource));
        }
    }
}
