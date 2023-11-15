using System.Threading.Tasks;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.ParticleSystems
{
    public class PvPDummyParticleSystemGroup : IPvPParticleSystemGroup
    {
        public async Task Play() { await Task.Yield(); }
        public async Task Stop() { await Task.Yield(); }
    }
}