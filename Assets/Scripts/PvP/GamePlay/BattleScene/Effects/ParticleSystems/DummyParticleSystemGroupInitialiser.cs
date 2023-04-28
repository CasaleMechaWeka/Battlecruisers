using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.ParticleSystems
{
    public class DummyParticleSystemGroupInitialiser : MonoBehaviour, IPvPParticleSystemGroupInitialiser
    {
        public IPvPParticleSystemGroup CreateParticleSystemGroup()
        {
            return new PvPDummyParticleSystemGroup();
        }
    }
}