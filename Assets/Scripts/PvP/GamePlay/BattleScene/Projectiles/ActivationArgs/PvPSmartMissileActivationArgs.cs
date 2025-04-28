using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs
{
    public class PvPProjectileActivationArgs : ProjectileActivationArgs
    {
        public IPvPCruiserTargetFactoriesProvider TargetFactories { get; }
        public IPvPCruiser EnempCruiser { get; }

        public PvPProjectileActivationArgs(
            Vector3 position,
            ProjectileStats projectileStats,
            Vector2 initialVelocityInMPerS,
            ITargetFilter targetFilter,
            ITarget parent,
            AudioClipWrapper impactSound,
            IPvPCruiserTargetFactoriesProvider targetFactories,
            IPvPCruiser enemyCruiser)
            : base(position, projectileStats, initialVelocityInMPerS, targetFilter, parent, impactSound)
        {
            Helper.AssertIsNotNull(targetFactories, enemyCruiser);

            TargetFactories = targetFactories;
            EnempCruiser = enemyCruiser;
        }
    }
}