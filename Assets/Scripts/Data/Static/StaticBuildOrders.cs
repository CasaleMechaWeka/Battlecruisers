using System.Collections.Generic;
using System.Collections.ObjectModel;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.Data.Static
{
    public static class StaticBuildOrders
    {
        public static ReadOnlyCollection<BuildingKey> Balanced = new ReadOnlyCollection<BuildingKey>(new List<BuildingKey>()
        {
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.ShieldGenerator,
            null,   //here we can insert offensives or other builidngs in BuildOrderFactory
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.ShieldGenerator,
            StaticPrefabKeys.Buildings.StealthGenerator,
            null,
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.ShieldGenerator,
            null,
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.ShieldGenerator,
            null,
            StaticPrefabKeys.Buildings.DroneStation4,
            StaticPrefabKeys.Buildings.ShieldGenerator,
            StaticPrefabKeys.Buildings.DroneStation4
        });

        public static ReadOnlyCollection<BuildingKey> Boom = new ReadOnlyCollection<BuildingKey>(new List<BuildingKey>()
        {
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.ShieldGenerator,
            StaticPrefabKeys.Buildings.ShieldGenerator,
            StaticPrefabKeys.Buildings.StealthGenerator,
            null,
            StaticPrefabKeys.Buildings.DroneStation4,
            StaticPrefabKeys.Buildings.DroneStation4,
            null,
            StaticPrefabKeys.Buildings.DroneStation4,
            StaticPrefabKeys.Buildings.ShieldGenerator,
            null,
            StaticPrefabKeys.Buildings.ShieldGenerator,
            null,
            StaticPrefabKeys.Buildings.ShieldGenerator,
            null,
            null,
        });

        public static ReadOnlyCollection<BuildingKey> FortressPrime = new ReadOnlyCollection<BuildingKey>(new List<BuildingKey>()
        {
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.DroneStation4,
            StaticPrefabKeys.Buildings.ShieldGenerator,
            StaticPrefabKeys.Buildings.StealthGenerator,
            null,
            StaticPrefabKeys.Buildings.DroneStation4,
            StaticPrefabKeys.Buildings.DroneStation4,
            StaticPrefabKeys.Buildings.DroneStation4,
            StaticPrefabKeys.Buildings.ShieldGenerator,
            null,
            StaticPrefabKeys.Buildings.DroneStation8,
            StaticPrefabKeys.Buildings.ShieldGenerator,
            null,
            StaticPrefabKeys.Buildings.ShieldGenerator,
            null,
            StaticPrefabKeys.Buildings.ShieldGenerator,
            null,
            null,
        });

        public static ReadOnlyCollection<BuildingKey> Rush = new ReadOnlyCollection<BuildingKey>(new List<BuildingKey>()
        {
            null,
            StaticPrefabKeys.Buildings.DroneStation,
            null,
            StaticPrefabKeys.Buildings.DroneStation,
            null,
            StaticPrefabKeys.Buildings.DroneStation,
            null,
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.DroneStation,
        });

    }
}
