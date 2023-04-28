using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.ParticleSystems
{
    public class PvPDummyParticleSystemGroupInitialiser : MonoBehaviour, IPvPParticleSystemGroupInitialiser
    {
        public IPvPParticleSystemGroup CreateParticleSystemGroup()
        {
            return new PvPDummyParticleSystemGroup();
        }
    }
}