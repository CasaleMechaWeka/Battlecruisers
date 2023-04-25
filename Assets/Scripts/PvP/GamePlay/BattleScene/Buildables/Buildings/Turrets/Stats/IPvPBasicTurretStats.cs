using System.Collections.ObjectModel;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers.FireInterval;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats
{
    public interface IPvPBasicTurretStats : IPvPDurationProvider
    {
        float FireRatePerS { get; }
        float RangeInM { get; }
        float MinRangeInM { get; }
        float MeanFireRatePerS { get; }
        ReadOnlyCollection<PvPTargetType> AttackCapabilities { get; }
    }
}
