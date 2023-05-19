using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.AudioSources;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction
{
    public class PvPUnitReadySignalInitialiser : MonoBehaviour
    {
        public IPvPManagedDisposable CreateSignal(IPvPCruiser parentCruiser)
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
                        new PvPAudioSourceBC(navalAudioSource),
                        /*parentCruiser.FactoryProvider.SettingsManager*/ null),
                    new PvPEffectVolumeAudioSource(
                        new PvPAudioSourceBC(aircraftAudioSource),
                        /*parentCruiser.FactoryProvider.SettingsManager*/ null));
        }
    }
}