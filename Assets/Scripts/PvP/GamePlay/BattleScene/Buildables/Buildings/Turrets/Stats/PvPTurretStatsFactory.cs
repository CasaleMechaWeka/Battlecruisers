using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats.Boosted;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System.Collections.ObjectModel;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats
{
    public class PvPTurretStatsFactory : ITurretStatsFactory
    {
        private readonly IGlobalBoostProviders _globalBoostProviders;

        public PvPTurretStatsFactory(IGlobalBoostProviders globalBoostProviders)
        {
            PvPHelper.AssertIsNotNull(globalBoostProviders);

            _globalBoostProviders = globalBoostProviders;
        }

        public ITurretStats CreateBoostedTurretStats(
            ITurretStats baseTurretStats,
            ObservableCollection<IBoostProvider> localBoostProviders,
            ObservableCollection<IBoostProvider> globalFireRateBoostProviders)
        {
            return new PvPBoostedTurretStats(baseTurretStats, localBoostProviders, globalFireRateBoostProviders, _globalBoostProviders);
        }
    }
}
