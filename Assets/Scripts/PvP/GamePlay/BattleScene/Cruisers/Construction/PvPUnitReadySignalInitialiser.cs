using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.AudioSources;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Audio;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction
{
    public class PvPUnitReadySignalInitialiser : MonoBehaviour
    {
        public IManagedDisposable CreateSignal(IPvPCruiser parentCruiser)
        {
            Assert.IsNotNull(parentCruiser);

            if (!parentCruiser.IsPlayerCruiser)
            {
                // Only play unit ready sounds for player cruiser
                return new PvPDummyManagedDisposable();
            }

            AudioSource navalAudioSource = transform.FindNamedComponent<AudioSource>("NavalAudioSource");
            AudioSource aircraftAudioSource = transform.FindNamedComponent<AudioSource>("AircraftAudioSource");

            return
                new PvPUnitReadySignal(
                    parentCruiser.UnitMonitor,
                    new PvPEffectVolumeAudioSource(
                        new AudioSourceBC(navalAudioSource),
                        parentCruiser.FactoryProvider.SettingsManager),
                    new PvPEffectVolumeAudioSource(
                        new AudioSourceBC(aircraftAudioSource),
                        parentCruiser.FactoryProvider.SettingsManager));
        }
    }
}