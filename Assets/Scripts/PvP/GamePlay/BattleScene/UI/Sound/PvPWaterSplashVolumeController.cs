using BattleCruisers.Data.Settings;
using Game2DWaterKit;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound
{
    public class PvPWaterSplashVolumeController : MonoBehaviour
    {
        private ISettingsManager _settingsManager;

        public Game2DWater water;

        public void Initialise(ISettingsManager settingsManager)
        {
            Assert.IsNotNull(water);
            Assert.IsNotNull(settingsManager);

            _settingsManager = settingsManager;

            _settingsManager.SettingsSaved += _settingsManager_SettingsSaved;

            SetVolume();
        }

        private void _settingsManager_SettingsSaved(object sender, EventArgs e)
        {
            SetVolume();
        }

        private void SetVolume()
        {
            water.OnCollisonRipplesModule.OnWaterEnterRipplesSoundEffect.AudioVolume = _settingsManager.EffectVolume * _settingsManager.MasterVolume;
        }
    }
}