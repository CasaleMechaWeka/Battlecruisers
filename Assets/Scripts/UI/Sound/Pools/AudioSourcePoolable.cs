using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using BattleCruisers.Utils.Threading;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Sound.Pools
{
    // FELIX  Use, test
    public class AudioSourcePoolable : IPoolable<AudioSourceActivationArgs>
    {
        private readonly IAudioSource _source;
        private readonly IDeferrer _deferrer;

        public event EventHandler Deactivated;

        public AudioSourcePoolable(IAudioSource source, IDeferrer deferrer)
        {
            Helper.AssertIsNotNull(source, deferrer);

            _source = source;
            _deferrer = deferrer;
        }

        public void Activate(AudioSourceActivationArgs activationArgs)
        {
            Assert.IsNotNull(activationArgs);

            _source.IsActive = true;
            _source.AudioClip = activationArgs.Sound;
            _source.Position = activationArgs.Position;
            _source.Play();

            _deferrer.Defer(CleanUp, activationArgs.Sound.AudioClip.length);
        }

        private void CleanUp()
        {
            _source.IsActive = false;
            Deactivated?.Invoke(this, EventArgs.Empty);
        }
    }
}