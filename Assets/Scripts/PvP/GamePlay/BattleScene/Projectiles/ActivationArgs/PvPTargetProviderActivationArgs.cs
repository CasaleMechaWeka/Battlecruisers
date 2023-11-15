using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs
{
    public class PvPTargetProviderActivationArgs<TPvPStats> : PvPProjectileActivationArgs<TPvPStats> where TPvPStats : IPvPProjectileStats
    {
        public IPvPTarget Target { get; }

        public PvPTargetProviderActivationArgs(
            Vector3 position,
            TPvPStats projectileStats,
            Vector2 initialVelocityInMPerS,
            IPvPTargetFilter targetFilter,
            IPvPTarget parent,
            IPvPAudioClipWrapper impactSound,
            IPvPTarget target)
            : base(position, projectileStats, initialVelocityInMPerS, targetFilter, parent, impactSound)
        {
            Assert.IsNotNull(target);
            Target = target;
        }
    }
}