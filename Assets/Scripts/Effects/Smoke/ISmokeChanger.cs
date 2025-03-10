using UnityEngine;

namespace BattleCruisers.Effects.Smoke
{
    public interface ISmokeChanger
    {
        void Change(ParticleSystem smoke, SmokeStatistics smokeStats);
    }
}