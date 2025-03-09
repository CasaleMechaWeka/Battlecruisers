using BattleCruisers.Effects.ParticleSystems;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.ParticleSystems
{
    public class PvPDummyBroadcastingParticleSystem : IBroadcastingParticleSystem
    {
#pragma warning disable 67  // Unused event
        public event EventHandler Stopped;
#pragma warning restore 67  // Unused event

        public void Play() { }
        public void Stop() { }
    }
}