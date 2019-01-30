using System;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Utilities
{
    /// <summary>
    /// To allow inspector to specify prefab keys.  Should be used to map to
    /// StaticPrefabKeys.
    /// </summary>
    public enum PrefabKeyName
    {
        // === Buildings ===
        // Factories
        Building_AirFactory,
        Building_NavalFactory,
        Building_EngineeringBay,

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

        // Offence
        Building_Artillery,
        Building_RocketLauncher,
        Building_Railgun,

        // Ultras
        Building_DeathstarLauncher,
        Building_NukeLauncher,
        Building_Ultralisk,
        Building_KamikazeSignal,
        Building_Broadsides,


        // === Units ===
        // Aircraft
        Unit_Bomber,
        Unit_Fighter,
        Unit_Gunship,

        // Ships
        Unit_AttackBoat,
        Unit_Frigate,
        Unit_Destroyer,
        Unit_ArchonBattleship,
    }

    // FELIX  Support hulls :)
    // FELIX  Move to NEW loadout screen namespace :P
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
                case "Building":
                    return GetPrefabKey<TKey>(typeof(StaticPrefabKeys.Buildings), keyName);

                case "Unit":
                    return GetPrefabKey<TKey>(typeof(StaticPrefabKeys.Units), keyName);

                default:
                    throw new ArgumentException();
            }
        }

        private static TKey GetPrefabKey<TKey>(Type type, string keyName) where TKey : IPrefabKey
        {
            return
                (TKey)type
                    .GetProperty(keyName)
                    .GetValue(obj: null, index: null);
        }
    }
}
