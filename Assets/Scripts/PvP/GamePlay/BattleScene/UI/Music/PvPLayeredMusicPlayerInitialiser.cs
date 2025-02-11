using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Utils.Audio;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using BattleCruisers.Utils;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Threading;
using BattleCruisers.UI.Music;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Music
{
    public class PvPLayeredMusicPlayerInitialiser : MonoBehaviour
    {
        public async Task<ILayeredMusicPlayer> CreatePlayerAsync(
            ISoundFetcher soundFetcher,
            SoundKeyPair soundKeys,
            ISettingsManager settingsManager)
        {
            PvPHelper.AssertIsNotNull(soundFetcher, soundKeys, settingsManager);

            AudioSource primarySource = transform.FindNamedComponent<AudioSource>("PrimaryAudioSource");
            IAudioClipWrapper primaryClip = await soundFetcher.GetSoundAsync(soundKeys.PrimaryKey);
            IAudioSource primary = new AudioSourceBC(primarySource)
            {
                AudioClip = primaryClip
            };

            AudioSource secondarySource = transform.FindNamedComponent<AudioSource>("SecondaryAudioSource");
            IAudioClipWrapper secondaryClip = await soundFetcher.GetSoundAsync(soundKeys.SecondaryKey);
            secondarySource.clip = secondaryClip.AudioClip;
            IAudioSource secondary = new AudioSourceBC(secondarySource)
            {
                AudioClip = secondaryClip
            };

            CoroutineStarter coroutineStarter = GetComponent<CoroutineStarter>();
            Assert.IsNotNull(coroutineStarter);

            return
                new LayeredMusicPlayer(
                    new AudioVolumeFade(coroutineStarter, PvPTimeBC.Instance),
                    primary,
                    secondary,
                    settingsManager);
        }
    }
}