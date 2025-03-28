using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BattleCruisers.Utils.Fetchers
{
    public static class SoundFetcher
    {
        private const string SOUND_ROOT_DIR = "Assets/Resources_moved/Sounds";
        private const char PATH_SEPARATOR = '/';

        public static async Task<AudioClipWrapper> GetSoundAsync(ISoundKey soundKey)
        {
            string soundPath = CreateSoundPath(soundKey);
            AsyncOperationHandle<AudioClip> handle = new AsyncOperationHandle<AudioClip>();
            //Debug.Log("[SoundFetcher] GetSoundAsync started with key: " + soundKey.Type + "/" + soundKey.Name);
            try
            {
                var validateAddress = Addressables.LoadResourceLocationsAsync(soundPath);
                await validateAddress.Task;
                Debug.Log("[SoundFetcher] ValidateAddress completed. Status: " + validateAddress.Status + ", Resource count: " + (validateAddress.Result != null ? validateAddress.Result.Count.ToString() : "null"));
                if (validateAddress.Status == AsyncOperationStatus.Succeeded)
                {
                    if (validateAddress.Result.Count > 0)
                    {
                        handle = Addressables.LoadAssetAsync<AudioClip>(soundPath);
                        await handle.Task;
                        Debug.Log("[SoundFetcher] LoadAssetAsync completed. Status: " + handle.Status + ", AudioClip is " + (handle.Result == null ? "null" : "valid"));

                        if (handle.Status != AsyncOperationStatus.Succeeded || handle.Result == null)
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
                Debug.Log(ex.Message + " === " + soundPath);
            }

            Debug.Log("[SoundFetcher] Returning AudioClipWrapper for sound: " + soundKey.Type + "/" + soundKey.Name);
            return new AudioClipWrapper(handle.Result, handle);
        }

        private static string CreateSoundPath(ISoundKey soundKey)
        {
            return SOUND_ROOT_DIR + PATH_SEPARATOR + soundKey.Type.ToString() + PATH_SEPARATOR + soundKey.Name;
        }
    }
}
