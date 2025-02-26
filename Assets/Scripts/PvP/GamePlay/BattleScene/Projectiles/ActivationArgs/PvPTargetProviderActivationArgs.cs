using BattleCruisers.Buildables;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs
{
    public class PvPTargetProviderActivationArgs<TPvPStats> : PvPProjectileActivationArgs<TPvPStats> where TPvPStats : IProjectileStats
    {
        public ITarget Target { get; }

        public PvPTargetProviderActivationArgs(
            Vector3 position,
            TPvPStats projectileStats,
            Vector2 initialVelocityInMPerS,
            ITargetFilter targetFilter,
            ITarget parent,
            IAudioClipWrapper impactSound,
            ITarget target)
            : base(position, projectileStats, initialVelocityInMPerS, targetFilter, parent, impactSound)
        {
            Assert.IsNotNull(target);
            Target = target;
        }
    }
}