using BattleCruisers.Utils.BattleScene.Pools;
using System;

namespace BattleCruisers.UI.Sound.Pools
{
    public class AudioSourcePoolable : IPoolable<AudioSourceActivationArgs>
    {
        public event EventHandler Deactivated;

        public void Activate(AudioSourceActivationArgs activationArgs)
        {
            throw new NotImplementedException();
        }
    }
}