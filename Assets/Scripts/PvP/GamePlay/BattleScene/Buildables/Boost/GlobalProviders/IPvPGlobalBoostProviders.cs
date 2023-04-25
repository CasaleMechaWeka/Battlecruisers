using System.Collections.ObjectModel;


namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost.GlobalProviders
{
    public interface IPvPGlobalBoostProviders
    {
        ObservableCollection<IPvPBoostProvider> DummyBoostProviders { get; }

        ObservableCollection<IPvPBoostProvider> AircraftBoostProviders { get; }

        ObservableCollection<IPvPBoostProvider> DefenseFireRateBoostProviders { get; }
        ObservableCollection<IPvPBoostProvider> OffenseFireRateBoostProviders { get; }
        // Currently affects ALL turrets (ships, gunship, buildings).  
        // That's ok though, because only used in tutorial to improve artillery accuracy :)
        ObservableCollection<IPvPBoostProvider> TurretAccuracyBoostProviders { get; }

        ObservableCollection<IPvPBoostProvider> ShieldRechargeRateBoostProviders { get; }

        IPvPBuildingBuildRatelBoostProviders BuildingBuildRate { get; }
        IPvPUnitBuildRatelBoostProviders UnitBuildRate { get; }
    }
}
