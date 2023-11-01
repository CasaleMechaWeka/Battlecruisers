using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.ProfileScreen
{
    public interface IVariant
    {
        Sprite variantSprite { get; }
        VariantType variantType { get; }
    }

    public enum VariantType
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
        AttackBoat,
        AttackRIB,
        Frigate,
        Destroyer,
        ArchonBattleship
    }
}
