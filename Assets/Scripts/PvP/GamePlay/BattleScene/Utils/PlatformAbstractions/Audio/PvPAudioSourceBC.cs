using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio
{
    public class PvPAudioSourceBC : IPvPAudioSource
    {
        private readonly AudioSource _audioSource;

        private const float MAX_BLEND = 1;
        private const float MIN_BLEND = 0;

        private IPvPAudioClipWrapper _audioClip;
        public IPvPAudioClipWrapper AudioClip
        {
            set
            {
                _audioClip = value;
                _audioSource.clip = value?.AudioClip;
            }
        }

        public bool IsPlaying => _audioSource.isPlaying;

        public float Volume
        {
            get => _audioSource.volume;
            set
            {
                // Check audio source has not been destroyed
                if (_audioSource != null)
                {
                    _audioSource.volume = value;
                }
            }
        }

        public Vector2 Position
        {
            get => _audioSource.transform.position;
            set
            {
                Vector3 newPosition = new Vector3(value.x, value.y, _audioSource.transform.position.z);
                _audioSource.transform.position = newPosition;
            }
        }

        public bool IsActive
        {
            get => _audioSource.gameObject.activeSelf;
            set
            {
                _audioSource.gameObject.SetActive(value);
            }
        }

        public PvPAudioSourceBC(AudioSource audioSource)
        {
            Assert.IsNotNull(audioSource);
            _audioSource = audioSource;
        }

        public void Play(bool isSpatial = true, bool loop = false)
        {
            _audioSource.spatialBlend = isSpatial ? MAX_BLEND : MIN_BLEND;
            _audioSource.loop = loop;
            _audioSource.Play();
        }

        public void Stop()
        {
            // Only stop audio if game object has not been destroyed
            if (_audioSource != null)
            {
                _audioSource.Stop();
            }
        }

        public void FreeAudioClip()
        {
            if (_audioClip != null)
            {
                Addressables.Release(_audioClip.Handle);
                _audioClip = null;
            }
        }
    }
}
