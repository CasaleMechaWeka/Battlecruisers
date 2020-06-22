using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Sound.Pools
{
    public class AudioSourceActivationArgs
    {
        public IAudioClipWrapper Sound { get; }
        public Vector2 Position { get; }

        public AudioSourceActivationArgs(IAudioClipWrapper sound, Vector2 position)
        {
            Assert.IsNotNull(sound);

            Sound = sound;
            Position = position;
        }

        public override bool Equals(object obj)
        {
            return
                obj is AudioSourceActivationArgs other
                && ReferenceEquals(Sound, other.Sound)
                && Position == other.Position;
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(Sound, Position);
        }
    }
}