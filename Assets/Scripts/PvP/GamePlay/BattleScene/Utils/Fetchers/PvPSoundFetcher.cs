using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using System;
using System.Threading.Tasks;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers
{
    public class PvPSoundFetcher : IPvPSoundFetcher
    {
        private const string SOUND_ROOT_DIR = "Assets/Resources_moved/Sounds";
        private const char PATH_SEPARATOR = '/';

        public async Task<IPvPAudioClipWrapper> GetSoundAsync(IPvPSoundKey soundKey)
        {
            string soundPath = CreateSoundPath(soundKey);
            AsyncOperationHandle<AudioClip> handle = new AsyncOperationHandle<AudioClip>();
            try
            {
                var validateAddress = Addressables.LoadResourceLocationsAsync(soundPath);
                await validateAddress.Task;
                if (validateAddress.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                {
                    if (validateAddress.Result.Count > 0)
                    {
                        handle = Addressables.LoadAssetAsync<AudioClip>(soundPath);
                        await handle.Task;

                        if (handle.Status != AsyncOperationStatus.Succeeded
                            || handle.Result == null)
                        {
                            throw new ArgumentException("Failed to retrieve sound with key: " + soundPath);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message + " === " + soundPath);
            }
            return new PvPAudioClipWrapper(handle.Result, handle);
        }

        private string CreateSoundPath(IPvPSoundKey soundKey)
        {
            return SOUND_ROOT_DIR + PATH_SEPARATOR + soundKey.Type.ToString() + PATH_SEPARATOR + soundKey.Name;
        }
    }
}
