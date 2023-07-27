using System.Threading.Tasks;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.ParticleSystems
{
    public interface IPvPParticleSystemGroup
    {
        Task Play();
        Task Stop();
    }
}