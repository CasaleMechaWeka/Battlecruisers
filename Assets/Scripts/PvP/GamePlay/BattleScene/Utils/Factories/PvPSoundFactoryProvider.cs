using BattleCruisers.UI.Sound.Players;
using BattleCruisers.UI.Sound.ProjectileSpawners;
using BattleCruisers.Utils.Factories;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories
{
    public class PvPSoundFactoryProvider : ISoundFactoryProvider
    {
        public IPrioritisedSoundPlayer IPrioritisedSoundPlayer { get; }
        public IPrioritisedSoundPlayer DummySoundPlayer { get; }
        public SingleSoundPlayer UISoundPlayer { get; }
        public SoundPlayerFactory SoundPlayerFactory { get; }

        public PvPSoundFactoryProvider(PvPBattleSceneGodComponents components)
        {
            PvPHelper.AssertIsNotNull(components);

            UISoundPlayer = new SingleSoundPlayer(components.UISoundsAudioSource);
            SoundPlayerFactory = new SoundPlayerFactory(components.Deferrer);
            DummySoundPlayer = new DummySoundPlayer();

            IPrioritisedSoundPlayer
                = new PrioritisedSoundPlayer(
                    new SingleSoundPlayer(
                        components.PrioritisedSoundPlayerAudioSource));
        }
    }
}
