using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BattleCruisers.Scenes.Test.Sounds
{
    public class ReleaseAudioClipTestGod : MonoBehaviour
    {

        public string audioClipPath = "Assets/Resources_moved/Sounds/Music/confusion-danger";

        void Start()
        {
            //TempAsync();
        }

        private AsyncOperationHandle<AudioClip> _handle;

        private async Task TempAsync()
        {
            AsyncOperationHandle<AudioClip> handle = Addressables.LoadAssetAsync<AudioClip>(audioClipPath);
            await handle.Task;
            // Expected ref count: 1  Actual:  7

            Addressables.Release(handle);
            // Expected ref count: 0  Actual:  3
        }

        public void LoadClip()
        {
            _handle = Addressables.LoadAssetAsync<AudioClip>(audioClipPath);
        }

        public void ReleaseClip()
        {
            Addressables.Release(_handle);
        }
    }
}