using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using UnityEngine;

namespace BattleCruisers.Cruisers.Construction
{
    public class UnitReadySignalInitialiser : MonoBehaviour
    {
        public IManagedDisposable CreateSignal(ICruiser parentCruiser, ISettingsManager settingsManager)
        {
            Helper.AssertIsNotNull(parentCruiser, settingsManager);

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
                    new AudioSourceBC(navalAudioSource),
                    new AudioSourceBC(aircraftAudioSource),
                    settingsManager);
        }
    }
}