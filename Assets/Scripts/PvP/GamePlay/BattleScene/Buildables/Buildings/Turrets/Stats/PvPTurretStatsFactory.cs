using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats.Boosted;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats
{
    public class PvPTurretStatsFactory : ITurretStatsFactory
    {
        private readonly GlobalBoostProviders _globalBoostProviders;

        public PvPTurretStatsFactory(GlobalBoostProviders globalBoostProviders)
        {
            PvPHelper.AssertIsNotNull(globalBoostProviders);

            _globalBoostProviders = globalBoostProviders;
        }

        public ITurretStats CreateBoostedTurretStats(
            ITurretStats baseTurretStats,
            ObservableCollection<IBoostProvider> localBoostProviders,
            List<ObservableCollection<IBoostProvider>> globalFireRateBoostProviders)
        {
            return new PvPBoostedTurretStats(baseTurretStats, localBoostProviders, globalFireRateBoostProviders, _globalBoostProviders);
        }
    }
}
