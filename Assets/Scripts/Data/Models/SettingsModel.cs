using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;
using System;
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
        public const int MIN_ZOOM_SPEED_LEVEL = 1;
        public const int MAX_ZOOM_SPEED_LEVEL = 9;

        public const int DEFAULT_SCROLL_SPEED_LEVEL = 5;
        public const int MIN_SCROLL_SPEED_LEVEL = 1;
        public const int MAX_SCROLL_SPEED_LEVEL = 9;

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
        private bool _muteMusic;
        public bool MuteMusic
        {
            get => _muteMusic;
            set => _muteMusic = value;
        }

        [SerializeField]
        private bool _muteVoices;
        public bool MuteVoices
        {
            get => _muteVoices;
            set => _muteVoices = value;
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
            AIDifficulty = Difficulty.Normal;
            ZoomSpeedLevel = DEFAULT_ZOOM_SPEED_LEVEL;
            ScrollSpeedLevel = DEFAULT_SCROLL_SPEED_LEVEL;
            MuteMusic = false;
            MuteVoices = false;
            ShowInGameHints = true;
        }

        public override bool Equals(object obj)
        {
            SettingsModel other = obj as SettingsModel;

            return
                other != null
                && AIDifficulty == other.AIDifficulty
                && MuteMusic == other.MuteMusic
                && MuteVoices == other.MuteVoices
                && ScrollSpeedLevel == other.ScrollSpeedLevel
                && ShowInGameHints == other.ShowInGameHints
                && ZoomSpeedLevel == other.ZoomSpeedLevel;
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(AIDifficulty, MuteMusic, MuteVoices, ScrollSpeedLevel, ShowInGameHints, ZoomSpeedLevel);
        }
    }
}