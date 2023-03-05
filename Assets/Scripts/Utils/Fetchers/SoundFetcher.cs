using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BattleCruisers.Utils.Fetchers
{
    public class SoundFetcher : ISoundFetcher
    {
        private const string SOUND_ROOT_DIR = "Assets/Resources_moved/Sounds";
        private const char PATH_SEPARATOR = '/';

        public async Task<IAudioClipWrapper> GetSoundAsync(ISoundKey soundKey)
        {
            string soundPath = CreateSoundPath(soundKey);       
            AsyncOperationHandle<AudioClip> handle = new AsyncOperationHandle<AudioClip>();
            try
            {
                handle = Addressables.LoadAssetAsync<AudioClip>(soundPath);
                await handle.Task;

                if (handle.Status != AsyncOperationStatus.Succeeded
                    || handle.Result == null)
                {
                    throw new ArgumentException("Failed to retrieve sound with key: " + soundPath);
                }                
            }
            catch(Exception ex)
            {
                Debug.Log(ex.Message + " === " + soundPath);
            }


            return new AudioClipWrapper(handle.Result, handle);
        }

        private string CreateSoundPath(ISoundKey soundKey)
        {
            return SOUND_ROOT_DIR + PATH_SEPARATOR + soundKey.Type.ToString() + PATH_SEPARATOR + soundKey.Name;
        }
    }
}
