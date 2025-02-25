using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using System.Collections.ObjectModel;


namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost.GlobalProviders
{
    public interface IPvPGlobalBoostProviders
    {
        ObservableCollection<IBoostProvider> DummyBoostProviders { get; }

        ObservableCollection<IBoostProvider> AircraftBoostProviders { get; }

        ObservableCollection<IBoostProvider> DefenseFireRateBoostProviders { get; }
        ObservableCollection<IBoostProvider> OffenseFireRateBoostProviders { get; }
        // Currently affects ALL turrets (ships, gunship, buildings).  
        // That's ok though, because only used in tutorial to improve artillery accuracy :)
        ObservableCollection<IBoostProvider> TurretAccuracyBoostProviders { get; }

        ObservableCollection<IBoostProvider> ShieldRechargeRateBoostProviders { get; }
        ObservableCollection<IBoostProvider> RocketBuildingsFireRateBoostProviders { get; }
        IBuildingBuildRatelBoostProviders BuildingBuildRate { get; }
        IUnitBuildRatelBoostProviders UnitBuildRate { get; }
        IBuildingHealthlBoostProviders BuildingHealth { get; }
    }
}
