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
        Hull_Trident,
        Hull_Raptor,
        Hull_Rockjaw,
        Hull_Hammerhead,
        Hull_Longbow,
        Hull_Megalodon,
        Hull_Eagle,
        Hull_Bullshark,
        Hull_TasDevil,
        Hull_Yeti,
        Hull_Rickshaw,
        Hull_BlackRig,

        // === Buildings ===
        // Factories
        Building_AirFactory,
        Building_NavalFactory,
        Building_DroneStation,
        Building_DroneStation4,
        Building_DroneStation8,

        // Tactical
        Building_ShieldGenerator,
        Building_StealthGenerator,
        Building_SpySatelliteLauncher,
        Building_LocalBooster,
        Building_ControlTower,

        // Defence
        Building_AntiShipTurret,
        Building_AntiAirTurret,
        Building_Mortar,
        Building_SamSite,
        Building_TeslaCoil,
        Building_Coastguard,

        // Offence
        Building_Artillery,
        Building_RocketLauncher,
        Building_Railgun,
        Building_MLRS,
        Building_GatlingMortar,
        Building_MissilePod,
        Building_IonCannon,

        // Ultras
        Building_DeathstarLauncher,
        Building_NukeLauncher,
        Building_Ultralisk,
        Building_KamikazeSignal,
        Building_Broadsides,
        Building_NovaArtillery,


        // === Units ===
        // Aircraft
        Unit_Bomber,
        Unit_Fighter,
        Unit_Gunship,
        Unit_SteamCopter,
        Unit_Broadsword,

        // Ships
        Unit_AttackBoat,
        Unit_Frigate,
        Unit_Destroyer,
        Unit_ArchonBattleship,
        Unit_AttackRIB,

        // CaptainExos
        CaptainExo_000,
        CaptainExo_001,
        CaptainExo_002,
        CaptainExo_003,
        CaptainExo_004,
        CaptainExo_005,
        CaptainExo_006,
        CaptainExo_007,
        CaptainExo_008,
        CaptainExo_009,
        CaptainExo_010,
        CaptainExo_011,
        CaptainExo_012,
        CaptainExo_013,
        CaptainExo_014,
        CaptainExo_015,
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
