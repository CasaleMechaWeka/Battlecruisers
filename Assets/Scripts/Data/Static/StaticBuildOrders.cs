using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BattleCruisers.Data.Models.PrefabKeys.Wrappers;

namespace BattleCruisers.Data.Static
{
    public static class StaticBuildOrders
    {
        public static class Adaptive
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

        public static class Basic
        {
            public static ReadOnlyCollection<IPrefabKeyWrapper> Balanced = new ReadOnlyCollection<IPrefabKeyWrapper>(new List<IPrefabKeyWrapper>()
            {
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                new AntiAirPrefabKeyWrapper(),
                new AntiNavalPrefabKeyWrapper(),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                new AntiAirPrefabKeyWrapper(),
                new AntiNavalPrefabKeyWrapper(),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.TeslaCoil),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.StealthGenerator),
                new OffensivePrefabKeyWrapper(),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                new OffensivePrefabKeyWrapper(),
                new AntiAirPrefabKeyWrapper(),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
                new OffensivePrefabKeyWrapper(),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                new OffensivePrefabKeyWrapper()
            });

            public static IList<IPrefabKeyWrapper> BoomAggressive
            {
                get
                {
                    List<IPrefabKeyWrapper> buildOrder = BoomCommon.ToList();

                    buildOrder.AddRange(new IPrefabKeyWrapper[]
                    {
                        new OffensivePrefabKeyWrapper(),
                        new OffensivePrefabKeyWrapper(),
                        new OffensivePrefabKeyWrapper(),
                        new OffensivePrefabKeyWrapper()
                    });

                    return buildOrder;
                }
            }

            public static IList<IPrefabKeyWrapper> BoomDefensive
            {
                get
                {
                    List<IPrefabKeyWrapper> buildOrder = BoomCommon.ToList();

                    buildOrder.AddRange(new IPrefabKeyWrapper[]
                    {
                        new OffensivePrefabKeyWrapper(),
                        new AntiAirPrefabKeyWrapper(),
                        new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.TeslaCoil),
                        new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
                        new OffensivePrefabKeyWrapper(),
                        new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
                        new OffensivePrefabKeyWrapper(),
                        new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
                        new OffensivePrefabKeyWrapper()
                    });

                    return buildOrder;
                }
            }

            public static ReadOnlyCollection<IPrefabKeyWrapper> BoomCommon = new ReadOnlyCollection<IPrefabKeyWrapper>(new List<IPrefabKeyWrapper>()
            {
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.StealthGenerator),
                new AntiAirPrefabKeyWrapper(),
                new AntiNavalPrefabKeyWrapper(),

                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                new AntiAirPrefabKeyWrapper(),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                new AntiNavalPrefabKeyWrapper()
            });

            public static ReadOnlyCollection<IPrefabKeyWrapper> BOOM = new ReadOnlyCollection<IPrefabKeyWrapper>(new List<IPrefabKeyWrapper>()
            {
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation4),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation4),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.StealthGenerator),
                new OffensivePrefabKeyWrapper(),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation8),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation8),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
                new OffensivePrefabKeyWrapper(),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation8),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
                new OffensivePrefabKeyWrapper(),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
                new OffensivePrefabKeyWrapper()
            });

            public static ReadOnlyCollection<IPrefabKeyWrapper> Rush = new ReadOnlyCollection<IPrefabKeyWrapper>(new List<IPrefabKeyWrapper>()
            {
                new OffensivePrefabKeyWrapper(),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                new AntiAirPrefabKeyWrapper(),
                new AntiNavalPrefabKeyWrapper(),
                new OffensivePrefabKeyWrapper(),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                new AntiAirPrefabKeyWrapper(),
                new AntiNavalPrefabKeyWrapper(),
                new OffensivePrefabKeyWrapper(),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                new OffensivePrefabKeyWrapper(),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                new OffensivePrefabKeyWrapper(),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation)
            });

            public static ReadOnlyCollection<IPrefabKeyWrapper> Turtle = new ReadOnlyCollection<IPrefabKeyWrapper>(new List<IPrefabKeyWrapper>()
            {
                new AntiAirPrefabKeyWrapper(),
                new AntiNavalPrefabKeyWrapper(),
                new AntiAirPrefabKeyWrapper(),
                new AntiNavalPrefabKeyWrapper(),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
                new AntiAirPrefabKeyWrapper(),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.StealthGenerator),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.TeslaCoil),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                new OffensivePrefabKeyWrapper(),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                new OffensivePrefabKeyWrapper(),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
                new OffensivePrefabKeyWrapper(),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
                new OffensivePrefabKeyWrapper(),
                new OffensivePrefabKeyWrapper(),
                new OffensivePrefabKeyWrapper(),
                new OffensivePrefabKeyWrapper()
            });

            public static ReadOnlyCollection<IPrefabKeyWrapper> FortressPrime = new ReadOnlyCollection<IPrefabKeyWrapper>(new List<IPrefabKeyWrapper>()
            {
                new AntiAirPrefabKeyWrapper(),
                new AntiNavalPrefabKeyWrapper(),
                new AntiAirPrefabKeyWrapper(),
                new AntiNavalPrefabKeyWrapper(),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation4),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation4),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation4),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.StealthGenerator),
                new AntiAirPrefabKeyWrapper(),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation4),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation4),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.TeslaCoil),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation8),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation8),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation8),
                new OffensivePrefabKeyWrapper(),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation8),
                new OffensivePrefabKeyWrapper(),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation8),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
                new OffensivePrefabKeyWrapper(),
                new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
                new OffensivePrefabKeyWrapper(),
                new OffensivePrefabKeyWrapper(),
                new OffensivePrefabKeyWrapper(),
                new OffensivePrefabKeyWrapper()
            });
        }
    }
}
