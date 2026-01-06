using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils
{
    /// <summary>
    /// To allow inspector to specify prefab keys.  Should be used to map to
    /// StaticPrefabKeys.
    /// </summary>
    public enum PrefabKeyName
    {
        None = -1,

        // === Hulls ===
        Hull_Trident = 0,
        Hull_Raptor = 1,
        Hull_Rockjaw = 2,
        Hull_Hammerhead = 3,
        Hull_Longbow = 4,
        Hull_Megalodon = 5,
        Hull_Eagle = 6,
        Hull_Bullshark = 7,
        Hull_TasDevil = 8,
        Hull_Yeti = 9,
        Hull_Rickshaw = 10,
        Hull_BlackRig = 0011,
        Hull_Flea = 0012,
        Hull_Shepherd = 0013,
        Hull_Microlodon = 0014,
        Hull_Pistol = 0015,
        Hull_Goatherd = 0016,
        Hull_Megalith = 0017,
        Hull_Cricket = 0018,

        Hull_BasicRig = 0019,
        Hull_FortNova = 0020,
        Hull_Zumwalt = 0021,
        Hull_Yucalux = 0022,
        Hull_TekGnosis = 0023,
        Hull_Salvage = 0024,
        Hull_Orac = 0025,
        Hull_Middlodon = 0026,
        Hull_Essex = 0027,
        Hull_Axiom = 0028,
        Hull_October = 0029,
        Hull_EndlessWall = 0030,
        Hull_AlphaSpace = 0031,
        Hull_Arkdeso = 0032,

        // === Buildings ===
        // Factories
        Building_AirFactory = 0100,
        Building_NavalFactory = 0101,
        Building_DroneStation = 0102,
        Building_DroneStation4 = 0103,
        Building_DroneStation6 = 0105,
        Building_DroneStation8 = 0104,
        Building_DroneFactory = 0106,

        // Tactical
        Building_ShieldGenerator = 0200,
        Building_StealthGenerator = 0201,
        Building_SpySatelliteLauncher = 0202,
        Building_LocalBooster = 0203,
        Building_ControlTower = 0204,
        Building_GrapheneBarrier = 0205,

        // Defence
        Building_AntiShipTurret = 0300,
        Building_AntiAirTurret = 0301,
        Building_Mortar = 0302,
        Building_SamSite = 0303,
        Building_TeslaCoil = 0304,
        Building_Coastguard = 0305,
        Building_FlakTurret = 0306,
        Building_CIWS = 0307,

        // Offence
        Building_Artillery = 0400,
        Building_RocketLauncher = 0401,
        Building_Railgun = 0402,
        Building_MLRS = 0403,
        Building_GatlingMortar = 0404,
        Building_MissilePod = 0405,
        Building_IonCannon = 0406,
        Building_Cannon = 0407,
        Building_BlastVLS = 0408,
        Building_FirecrackerVLS = 0409,

        // Ultras
        Building_DeathstarLauncher = 0500,
        Building_NukeLauncher = 0501,
        Building_Ultralisk = 0502,
        Building_KamikazeSignal = 0503,
        Building_Broadsides = 0504,
        Building_NovaArtillery = 0505,
        Building_UltraCIWS = 0506,
        Building_GlobeShield = 0507,
        Building_Sledgehammer = 0508,
        Building_RailCannon = 0509,

        // === Units ===
        // Aircraft
        Unit_Bomber = 0600,
        Unit_Fighter = 0601,
        Unit_Gunship = 0602,
        Unit_SteamCopter = 0603,
        Unit_Broadsword = 0604,
        Unit_StratBomber = 0605,
        Unit_SpyPlane = 0606,
        Unit_MissileFighter = 0607,

        // Ships
        Unit_AttackBoat = 0700,
        Unit_Frigate = 0701,
        Unit_Destroyer = 0702,
        Unit_SiegeDestroyer = 0703,
        Unit_ArchonBattleship = 0704,
        Unit_AttackRIB = 0705,
        Unit_GlassCannoneer = 0706,
        Unit_GunBoat = 0707,
        Unit_RocketTurtle = 0708,
        Unit_FlakTurtle = 0709,
        Unit_TeslaTurtle = 0710
    }

    public static class StaticPrefabKeyHelper
    {
        private const char SEPARATOR = '_';

        public static TKey GetPrefabKey<TKey>(PrefabKeyName prefabKeyName) where TKey : IPrefabKey
        {
            string keyNameStr = prefabKeyName.ToString();

            string[] strAsArray = keyNameStr.Split(SEPARATOR);
            Assert.AreEqual(2, strAsArray.Length);

            string keyType = strAsArray[0];
            string keyName = strAsArray[1];

            return keyType switch
            {
                "Hull"       => GetPrefabKey<TKey>(typeof(StaticPrefabKeys.Hulls), keyName),
                "Building"   => GetPrefabKey<TKey>(typeof(StaticPrefabKeys.Buildings), keyName),
                "Unit"       => GetPrefabKey<TKey>(typeof(StaticPrefabKeys.Units), keyName),
                "CaptainExo" => GetPrefabKey<TKey>(typeof(StaticPrefabKeys.CaptainExos), keyName),
                _ => throw new ArgumentException(),
            };

        }

        private static TKey GetPrefabKey<TKey>(Type type, string keyName) where TKey : IPrefabKey
        {
            Logging.Log(Tags.PREFAB_KEY_HELPER, keyName);

            return
                (TKey)type
                    .GetProperty(keyName)
                    .GetValue(obj: null, index: null);
        }
    }
}
