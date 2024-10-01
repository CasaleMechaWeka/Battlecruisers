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
        // === Hulls ===
        Hull_Trident = 0000,
        Hull_Raptor = 0001,
        Hull_Rockjaw = 0002,
        Hull_Hammerhead = 0003,
        Hull_Longbow = 0004,
        Hull_Megalodon = 0005,
        Hull_Eagle = 0006,
        Hull_Bullshark = 0007,
        Hull_TasDevil = 0008,
        Hull_Yeti = 0009,
        Hull_Rickshaw = 0010,
        Hull_BlackRig = 0011,
        Hull_Flea = 0012,
        Hull_Shepherd = 0013,
        Hull_Microlodon = 0014,
        Hull_Pistol = 0015,
        Hull_Goatherd = 0016,
        Hull_Megalith = 0017,

        // === Buildings ===
        // Factories
        Building_AirFactory = 0100,
        Building_NavalFactory = 0101,
        Building_DroneStation = 0102,
        Building_DroneStation4 = 0103,
        Building_DroneStation6 = 0105,
        Building_DroneStation8 = 0104,

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


        // === Units ===
        // Aircraft
        Unit_Bomber = 0600,
        Unit_Fighter = 0601,
        Unit_Gunship = 0602,
        Unit_SteamCopter = 0603,
        Unit_Broadsword = 0604,
        Unit_StratBomber = 0605,
        Unit_SpyPlane = 0606,

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
        Unit_FlakTurtle = 0709
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

            switch (keyType)
            {
                case "Hull":
                    return GetPrefabKey<TKey>(typeof(StaticPrefabKeys.Hulls), keyName);

                case "Building":
                    return GetPrefabKey<TKey>(typeof(StaticPrefabKeys.Buildings), keyName);

                case "Unit":
                    return GetPrefabKey<TKey>(typeof(StaticPrefabKeys.Units), keyName);

                case "CaptainExo":
                    return GetPrefabKey<TKey>(typeof(StaticPrefabKeys.CaptainExos), keyName);

                default:
                    throw new ArgumentException();
            }
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
