using BattleCruisers.Effects.Smoke;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Smoke
{
    public interface IPvPSmokeChanger
    {
        void Change(ParticleSystem smoke, SmokeStatistics smokeStats);
    }
}