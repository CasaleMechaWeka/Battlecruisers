using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost.GlobalProviders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats.Boosted;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System.Collections.ObjectModel;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats
{
    public class PvPTurretStatsFactory : IPvPTurretStatsFactory
    {
        private readonly IPvPBoostFactory _boostFactory;
        private readonly IPvPGlobalBoostProviders _globalBoostProviders;

        public PvPTurretStatsFactory(IPvPBoostFactory boostFactory, IPvPGlobalBoostProviders globalBoostProviders)
        {
            PvPHelper.AssertIsNotNull(boostFactory, globalBoostProviders);

            _boostFactory = boostFactory;
            _globalBoostProviders = globalBoostProviders;
        }

        public ITurretStats CreateBoostedTurretStats(
            ITurretStats baseTurretStats,
            ObservableCollection<IBoostProvider> localBoostProviders,
            ObservableCollection<IBoostProvider> globalFireRateBoostProviders)
        {
            return new PvPBoostedTurretStats(baseTurretStats, _boostFactory, localBoostProviders, globalFireRateBoostProviders, _globalBoostProviders);
        }
    }
}
