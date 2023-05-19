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

        public IPvPTurretStats CreateBoostedTurretStats(
            IPvPTurretStats baseTurretStats,
            ObservableCollection<IPvPBoostProvider> localBoostProviders,
            ObservableCollection<IPvPBoostProvider> globalFireRateBoostProviders)
        {
            return new PvPBoostedTurretStats(baseTurretStats, _boostFactory, localBoostProviders, globalFireRateBoostProviders, _globalBoostProviders);
        }
    }
}
