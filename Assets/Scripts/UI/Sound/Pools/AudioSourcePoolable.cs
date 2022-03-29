using BattleCruisers.Buildables;
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

        public event EventHandler Deactivated;

        public AudioSourcePoolable(IAudioSource source, IDeferrer realTimeDeferrer)
        {
            Helper.AssertIsNotNull(source, realTimeDeferrer);

            _source = source;
            _realTimeDeferrer = realTimeDeferrer;

            // Do not activate until we are ready to play
            _source.IsActive = false;
        }

        public void Activate(AudioSourceActivationArgs activationArgs)
        {
            Assert.IsNotNull(activationArgs);

            _source.IsActive = true;
            _source.AudioClip = activationArgs.Sound;
            _source.Position = activationArgs.Position;
            _source.Play();

            _realTimeDeferrer.Defer(CleanUp, activationArgs.Sound.Length);
        }

        public void Activate(AudioSourceActivationArgs activationArgs, Faction faction)
        {
        }

        private void CleanUp()
        {
            _source.IsActive = false;
            Deactivated?.Invoke(this, EventArgs.Empty);
        }
    }
}