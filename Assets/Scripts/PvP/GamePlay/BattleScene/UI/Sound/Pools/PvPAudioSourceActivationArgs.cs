using BattleCruisers.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Pools
{
    public class PvPAudioSourceActivationArgs
    {
        public IPvPAudioClipWrapper Sound { get; }
        public Vector2 Position { get; }

        public PvPAudioSourceActivationArgs(IPvPAudioClipWrapper sound, Vector2 position)
        {
            Assert.IsNotNull(sound);

            Sound = sound;
            Position = position;
        }

        public override bool Equals(object obj)
        {
            return
                obj is PvPAudioSourceActivationArgs other
                && ReferenceEquals(Sound, other.Sound)
                && Position == other.Position;
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(Sound, Position);
        }
    }
}