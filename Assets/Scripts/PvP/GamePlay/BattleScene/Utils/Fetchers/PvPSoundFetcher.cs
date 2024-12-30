using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using BattleCruisers.UI.Sound;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers
{
    public class PvPSoundFetcher : IPvPSoundFetcher
    {
        private const string SOUND_ROOT_DIR = "Assets/Resources_moved/Sounds";
        private const char PATH_SEPARATOR = '/';

        public async Task<IPvPAudioClipWrapper> GetSoundAsync(ISoundKey soundKey)
        {
            string soundPath = CreateSoundPath(soundKey);
            AudioClip clip = null;
            AsyncOperationHandle<AudioClip> handle = new AsyncOperationHandle<AudioClip>();
            try
            {
                var validateAddress = Addressables.LoadResourceLocationsAsync(soundPath);
                await validateAddress.Task;
                if (validateAddress.Status == AsyncOperationStatus.Succeeded)
                {
                    if (validateAddress.Result.Count > 0)
                    {
                        handle = Addressables.LoadAssetAsync<AudioClip>(soundPath);
                        clip = await handle.Task;

                        if (handle.Status != AsyncOperationStatus.Succeeded || clip == null)
                        {
                            throw new ArgumentException("Failed to retrieve sound");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Failed to retrieve sound: address didn't contain a valid sound");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message + " === " + soundPath);
            }

            return new PvPAudioClipWrapper(clip);
        }

        private string CreateSoundPath(ISoundKey soundKey)
        {
            return SOUND_ROOT_DIR + PATH_SEPARATOR + soundKey.Type.ToString() + PATH_SEPARATOR + soundKey.Name;
        }
    }
}
