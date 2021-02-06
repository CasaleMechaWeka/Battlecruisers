using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Utils.Threading;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Sound.Pools
{
    public class AudioSourcePoolable : IAudioSourcePoolable
    {
        private readonly IAudioSource _source;
        private readonly IDeferrer _realTimeDeferrer;
        private readonly ISettingsManager _settingsManager;

        public event EventHandler Deactivated;

        public AudioSourcePoolable(IAudioSource source, IDeferrer realTimeDeferrer, ISettingsManager settingsManager)
        {
            Helper.AssertIsNotNull(source, realTimeDeferrer, settingsManager);

            _source = source;
            _realTimeDeferrer = realTimeDeferrer;
            _settingsManager = settingsManager;

            // Do not activate until we are ready to play
            _source.IsActive = false;
        }

        public void Activate(AudioSourceActivationArgs activationArgs)
        {
            Assert.IsNotNull(activationArgs);

            _source.IsActive = true;
            _source.AudioClip = activationArgs.Sound;
            _source.Position = activationArgs.Position;
            _source.Volume = _settingsManager.EffectVolume;
            _source.Play();

            _settingsManager.SettingsSaved += _settingsManager_SettingsSaved;
            _realTimeDeferrer.Defer(CleanUp, activationArgs.Sound.Length);
        }

        private void CleanUp()
        {
            _settingsManager.SettingsSaved -= _settingsManager_SettingsSaved;
            _source.IsActive = false;
            Deactivated?.Invoke(this, EventArgs.Empty);
        }

        private void _settingsManager_SettingsSaved(object sender, EventArgs e)
        {
            _settingsManager.EffectVolume = _settingsManager.EffectVolume;
        }
    }
}