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
        AirFactory = 0100,
        NavalFactory = 0101,
        EngineeringBay = 0102,
        EngineeringBay4 = 0103,
        EngineeringBay8 = 0104,

        ShieldGenerator = 0200,
        StealthGenerator = 0201,
        SpySatelliteLauncher = 0202,
        LocalBooster = 0203,
        ControlTower = 0204,
        GrapheneBarrier = 0205,

        AntiShipTurret = 0300,
        AntiAirTurret = 0301,
        Mortar = 0302,
        SamSite = 0303,
        TeslaCoil = 0304,
        Coastguard = 0305,
        FlakTurret = 0306,
        CIWS = 0307,

        Artillery = 0400,
        RocketLauncher = 0401,
        Railgun = 0402,
        MLRS = 0403,
        GatlingMortar = 0404,
        IonCannon = 0405,
        MissilePod = 0406,
        Cannon = 0407,
        BlastVLS = 0408,
        FirecrackerVLS = 0409,

        Bomber = 0500,              //anything with a higher index than this should be a Unit, otherwise it breaks the
        Fighter = 0501,             //IsUnit() function in 'VariantPrefab.cs'
        Gunship = 0502,
        SteamCopter = 0503,
        Broadsword = 0504,
        StratBomber = 0505,
        SpyPlane = 0506,
        MissileFighter = 0507,

        AttackBoat = 0600,
        AttackRIB = 0601,
        Frigate = 0602,
        Destroyer = 0603,
        SiegeDestroyer = 0604,
        ArchonBattleship = 0605,
        GlassCannoneer = 0606,
        GunBoat = 0607,
        RocketTurtle = 0608,
        FlakTurtle = 0609,
        
    }
}
