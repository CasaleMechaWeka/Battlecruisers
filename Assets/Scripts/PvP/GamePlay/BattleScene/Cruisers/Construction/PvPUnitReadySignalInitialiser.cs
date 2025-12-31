using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using BattleCruisers.UI.Sound.AudioSources;

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
                return new DummyManagedDisposable();
            }

            AudioSource navalAudioSource = transform.FindNamedComponent<AudioSource>("NavalAudioSource");
            AudioSource aircraftAudioSource = transform.FindNamedComponent<AudioSource>("AircraftAudioSource");

            return
                new PvPUnitReadySignal(
                    parentCruiser.UnitMonitor,
                    new EffectVolumeAudioSource(
                        new AudioSourceBC(navalAudioSource)),
                    new EffectVolumeAudioSource(
                        new AudioSourceBC(aircraftAudioSource)));
        }
    }
}