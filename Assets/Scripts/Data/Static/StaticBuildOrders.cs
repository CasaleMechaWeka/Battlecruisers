using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BattleCruisers.Data.Models.PrefabKeys.Wrappers;

namespace BattleCruisers.Data.Static
{
    public static class StaticBuildOrders
    {
        public static ReadOnlyCollection<IPrefabKeyWrapper> Balanced = new ReadOnlyCollection<IPrefabKeyWrapper>(new List<IPrefabKeyWrapper>()
        {
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
            new OffensivePrefabKeyWrapper(),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.StealthGenerator),
            new OffensivePrefabKeyWrapper(),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
            new OffensivePrefabKeyWrapper(),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
            new OffensivePrefabKeyWrapper(),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation4),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation4)
        });

        public static ReadOnlyCollection<IPrefabKeyWrapper> Boom = new ReadOnlyCollection<IPrefabKeyWrapper>(new List<IPrefabKeyWrapper>()
        {
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.StealthGenerator),
            new OffensivePrefabKeyWrapper(),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation4),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation4),
            new OffensivePrefabKeyWrapper(),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation4),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
            new OffensivePrefabKeyWrapper(),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
            new OffensivePrefabKeyWrapper(),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
            new OffensivePrefabKeyWrapper(),
            new OffensivePrefabKeyWrapper(),
        });

        public static ReadOnlyCollection<IPrefabKeyWrapper> FortressPrime = new ReadOnlyCollection<IPrefabKeyWrapper>(new List<IPrefabKeyWrapper>()
        {
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation4),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.StealthGenerator),
            new OffensivePrefabKeyWrapper(),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation4),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation4),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation4),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
            new OffensivePrefabKeyWrapper(),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation8),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
            new OffensivePrefabKeyWrapper(),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
            new OffensivePrefabKeyWrapper(),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
            new OffensivePrefabKeyWrapper(),
            new OffensivePrefabKeyWrapper(),
        });

        public static ReadOnlyCollection<IPrefabKeyWrapper> Rush = new ReadOnlyCollection<IPrefabKeyWrapper>(new List<IPrefabKeyWrapper>()
        {
            new OffensivePrefabKeyWrapper(),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
            new OffensivePrefabKeyWrapper(),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
            new OffensivePrefabKeyWrapper(),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
            new OffensivePrefabKeyWrapper(),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
            new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation)
        });

    }
}
