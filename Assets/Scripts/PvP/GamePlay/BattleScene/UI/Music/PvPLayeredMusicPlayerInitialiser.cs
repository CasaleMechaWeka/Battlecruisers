using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Audio;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading;
using BattleCruisers.Utils;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Music
{
    public class PvPLayeredMusicPlayerInitialiser : MonoBehaviour
    {
        public async Task<IPvPLayeredMusicPlayer> CreatePlayerAsync(
            ISoundFetcher soundFetcher,
            SoundKeyPair soundKeys,
            ISettingsManager settingsManager)
        {
            PvPHelper.AssertIsNotNull(soundFetcher, soundKeys, settingsManager);

            AudioSource primarySource = transform.FindNamedComponent<AudioSource>("PrimaryAudioSource");
            IAudioClipWrapper primaryClip = await soundFetcher.GetSoundAsync(soundKeys.PrimaryKey);
            IAudioSource primary = new PvPAudioSourceBC(primarySource)
            {
                AudioClip = primaryClip
            };

            AudioSource secondarySource = transform.FindNamedComponent<AudioSource>("SecondaryAudioSource");
            IAudioClipWrapper secondaryClip = await soundFetcher.GetSoundAsync(soundKeys.SecondaryKey);
            secondarySource.clip = secondaryClip.AudioClip;
            IAudioSource secondary = new PvPAudioSourceBC(secondarySource)
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