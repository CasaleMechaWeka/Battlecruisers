using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs
{
    public class PvPSmartMissileActivationArgs<TPvPStats> : PvPProjectileActivationArgs<TPvPStats> where TPvPStats : IPvPProjectileStats
    {
        public IPvPCruiserTargetFactoriesProvider TargetFactories { get; }
        public IPvPCruiser EnempCruiser { get; }

        public PvPSmartMissileActivationArgs(
            Vector3 position,
            TPvPStats projectileStats,
            Vector2 initialVelocityInMPerS,
            IPvPTargetFilter targetFilter,
            ITarget parent,
            IAudioClipWrapper impactSound,
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