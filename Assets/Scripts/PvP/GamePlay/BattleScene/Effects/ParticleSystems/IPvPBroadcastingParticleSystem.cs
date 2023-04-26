using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.ParticleSystems
{
    public interface IPvPBroadcastingParticleSystem
    {
        event EventHandler Stopped;

        void Play();
        void Stop();
    }
}