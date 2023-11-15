using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Audio;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading;
using System.Threading.Tasks;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Music
{
    public class PvPLayeredMusicPlayerInitialiser : MonoBehaviour
    {
        public async Task<IPvPLayeredMusicPlayer> CreatePlayerAsync(
            IPvPSoundFetcher soundFetcher,
            PvPSoundKeyPair soundKeys,
            ISettingsManager settingsManager)
        {
            PvPHelper.AssertIsNotNull(soundFetcher, soundKeys, settingsManager);

            AudioSource primarySource = transform.FindNamedComponent<AudioSource>("PrimaryAudioSource");
            IPvPAudioClipWrapper primaryClip = await soundFetcher.GetSoundAsync(soundKeys.PrimaryKey);
            IPvPAudioSource primary = new PvPAudioSourceBC(primarySource)
            {
                AudioClip = primaryClip
            };

            AudioSource secondarySource = transform.FindNamedComponent<AudioSource>("SecondaryAudioSource");
            IPvPAudioClipWrapper secondaryClip = await soundFetcher.GetSoundAsync(soundKeys.SecondaryKey);
            secondarySource.clip = secondaryClip.AudioClip;
            IPvPAudioSource secondary = new PvPAudioSourceBC(secondarySource)
            {
                AudioClip = secondaryClip
            };

            PvPCoroutineStarter coroutineStarter = GetComponent<PvPCoroutineStarter>();
            Assert.IsNotNull(coroutineStarter);

            return
                new PvPLayeredMusicPlayer(
                    new PvPAudioVolumeFade(coroutineStarter, PvPTimeBC.Instance),
                    primary,
                    secondary,
                    settingsManager);
        }
    }
}