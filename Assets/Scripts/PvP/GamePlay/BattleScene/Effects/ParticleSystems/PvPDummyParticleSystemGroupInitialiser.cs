using BattleCruisers.Effects.ParticleSystems;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.ParticleSystems
{
    public class PvPDummyParticleSystemGroupInitialiser : MonoBehaviour, IParticleSystemGroupInitialiser
    {
        public IParticleSystemGroup CreateParticleSystemGroup()
        {
            return new PvPDummyParticleSystemGroup();
        }
    }
}