using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.ProfileScreen
{
    public interface IVariantPrefab
    {
        int variantIndex { get; }
        Sprite variantSprite { get; }
        ParentBuildable parent { get; set; }
        VariantType variantType { get; }
    }

    public enum VariantType
    {
        Stats,
        Undefined
    }
    public enum ParentBuildable
    {
        AirFactory,
        NavalFactory,
        EngineeringBay,
        EngineeringBay4,
        EngineeringBay8,
        ShieldGenerator,
        StealthGenerator,
        SpySatelliteLauncher,
        LocalBooster,
        ControlTower,
        AntiShipTurret,
        AntiAirTurret,
        Mortar,
        SamSite,
        TeslaCoil,
        Coastguard,
        FlakTurret,
        CIWS,
        Artillery,
        RocketLauncher,
        Railgun,
        MLRS,
        GatlingMortar,
        IonCannon,
        MissilePod,
        Bomber,
        Fighter,
        Gunship,
        SteamCopter,
        Broadsword,
        StratBomber,
        AttackBoat,
        AttackRIB,
        Frigate,
        Destroyer,
        SiegeDestroyer,
        ArchonBattleship,
        GlassCannoneer,
        GunBoat,
        RocketTurtle,
        GlobeShield,
        GrapheneBarrier
    }
}
