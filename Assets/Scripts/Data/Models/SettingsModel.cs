using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;
using System;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Data.Models
{
    // Serialise settings instead of using UnityEngine.PlayerPrefs, so that
    // settings can be cloud synced via steam.
    [Serializable]
    public class SettingsModel : ISettingsModel
    {
        private const int DEFAULT_ZOOM_SPEED_LEVEL = 5;
        public const int MIN_ZOOM_SPEED_LEVEL = 2;
        public const int MAX_ZOOM_SPEED_LEVEL = 9;

        public const int DEFAULT_SCROLL_SPEED_LEVEL = 5;
        public const int MIN_SCROLL_SPEED_LEVEL = 2;
        public const int MAX_SCROLL_SPEED_LEVEL = 9;

        public const float DEFAULT_VOLUME = 0.5f;
        public const float MIN_VOLUME = 0;
        public const float MAX_VOLUME = 1;

        [SerializeField]
        private int _version;
        public int Version
        {
            get => _version;
            set => _version = value;
        }

        public class ModelVersion
        {
            public const int PreMusicVolume = 0;
            public const int WithMusicVolume = 1;
            public const int RemovedEasyDifficulty = 2;
        }

        [SerializeField]
        private Difficulty _aiDifficulty;
        public Difficulty AIDifficulty
        {
            get => _aiDifficulty;
            set => _aiDifficulty = value;
        }

        [SerializeField]
        private int _zoomSpeedLevel;
        public int ZoomSpeedLevel
        {
            get => _zoomSpeedLevel;
            set
            {
                Assert.IsTrue(value >= MIN_ZOOM_SPEED_LEVEL);
                Assert.IsTrue(value <= MAX_ZOOM_SPEED_LEVEL);
                _zoomSpeedLevel = value;
            }
        }

        [SerializeField]
        private int _scrollSpeedLevel;
        public int ScrollSpeedLevel
        {
            get => _scrollSpeedLevel;
            set
            {
                Assert.IsTrue(value >= MIN_SCROLL_SPEED_LEVEL);
                Assert.IsTrue(value <= MAX_SCROLL_SPEED_LEVEL);
                _scrollSpeedLevel = value;
            }
        }

        [SerializeField]
        private float _musicVolume;
        public float MusicVolume
        {
            get => _musicVolume;
            set
            {
                CheckVolumeValue(value);
                _musicVolume = value;
            }
        }

        [SerializeField]
        private float _effectVolume;
        public float EffectVolume
        {
            get => _effectVolume;
            set
            {
                CheckVolumeValue(value);
                _effectVolume = value;
            }
        }

        [SerializeField]
        private bool _showInGameHints;
        public bool ShowInGameHints
        {
            get => _showInGameHints;
            set => _showInGameHints = value;
        }

        public SettingsModel()
        {
            AIDifficulty = Difficulty.Hard;
            ZoomSpeedLevel = DEFAULT_ZOOM_SPEED_LEVEL;
            ScrollSpeedLevel = DEFAULT_SCROLL_SPEED_LEVEL;
            MusicVolume = DEFAULT_VOLUME;
            EffectVolume = DEFAULT_VOLUME;
            ShowInGameHints = true;
        }

        private void CheckVolumeValue(float volume)
        {
            Assert.IsTrue(volume >= MIN_VOLUME);
            Assert.IsTrue(volume <= MAX_VOLUME);
        }

        [OnDeserialized()]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            // For backwards compatability, when this class did not have these fields
            if (_version == ModelVersion.PreMusicVolume)
            {
                _musicVolume = DEFAULT_VOLUME;
                _effectVolume = DEFAULT_VOLUME;

                _version = ModelVersion.WithMusicVolume;
            }
            else if (_version == ModelVersion.WithMusicVolume)
            {
                if (_aiDifficulty == Difficulty.Easy)
                {
                    _aiDifficulty = Difficulty.Normal;
                }

                _version = ModelVersion.RemovedEasyDifficulty;
            }
        }

        public override bool Equals(object obj)
        {
            SettingsModel other = obj as SettingsModel;

            return
                other != null
                && AIDifficulty == other.AIDifficulty
                && ScrollSpeedLevel == other.ScrollSpeedLevel
                && ShowInGameHints == other.ShowInGameHints
                && ZoomSpeedLevel == other.ZoomSpeedLevel
                && MusicVolume == other.MusicVolume
                && EffectVolume == other.EffectVolume;
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(AIDifficulty, ScrollSpeedLevel, ShowInGameHints, ZoomSpeedLevel, MusicVolume, EffectVolume);
        }
    }
}