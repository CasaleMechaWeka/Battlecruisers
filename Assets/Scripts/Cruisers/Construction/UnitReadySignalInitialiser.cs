using BattleCruisers.UI.Sound.AudioSources;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Construction
{
    public class UnitReadySignalInitialiser : MonoBehaviour
    {
        public IManagedDisposable CreateSignal(ICruiser parentCruiser)
        {
            Assert.IsNotNull(parentCruiser);

            if (!parentCruiser.IsPlayerCruiser)
            {
                // Only play unit ready sounds for player cruiser
                return new DummyManagedDisposable();
            }

            AudioSource navalAudioSource = transform.FindNamedComponent<AudioSource>("NavalAudioSource");
            AudioSource aircraftAudioSource = transform.FindNamedComponent<AudioSource>("AircraftAudioSource");

            return
                new UnitReadySignal(
                    parentCruiser.UnitMonitor,
                    new EffectVolumeAudioSource(
                        new AudioSourceBC(navalAudioSource),
                        parentCruiser.FactoryProvider.SettingsManager),
                    new EffectVolumeAudioSource(
                        new AudioSourceBC(aircraftAudioSource),
                        parentCruiser.FactoryProvider.SettingsManager));
        }
    }
}