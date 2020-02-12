using System.Collections.Generic;
using BattleCruisers.Data.Models.PrefabKeys.Wrappers;

namespace BattleCruisers.Data.Static
{
    public static class StaticBuildOrders
	{
        public static class Adaptive
        {
			public static IList<IPrefabKeyWrapper> Balanced
			{
				get
				{
					return new List<IPrefabKeyWrapper>()
					{
						new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
						new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
						new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
                        new OffensivePrefabKeyWrapper(),
						new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                        new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.StealthGenerator),
						new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
						new OffensivePrefabKeyWrapper(),
						new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                        new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
                        new OffensivePrefabKeyWrapper(),
                        new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                        new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
                        new OffensivePrefabKeyWrapper(),
                        new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                        new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
                        new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation)
                    };
				}
			}
			
            public static IList<IPrefabKeyWrapper> Boom
			{
				get
				{
					return new List<IPrefabKeyWrapper>()
					{
						new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
						new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
						new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                        new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
						new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.StealthGenerator),
						new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                        new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                        new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
						new OffensivePrefabKeyWrapper(),
                        new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                        new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                        new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
						new OffensivePrefabKeyWrapper(),
                        new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
						new OffensivePrefabKeyWrapper(),
                        new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
						new OffensivePrefabKeyWrapper()
                    };
				}
			}
			
            public static IList<IPrefabKeyWrapper> Rush
			{
				get
				{
					return new List<IPrefabKeyWrapper>()
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
						new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation)
					};
				}
			}
        }

        public static class Basic
        {
			public static IList<IPrefabKeyWrapper> Balanced
			{
				get
				{
					return new List<IPrefabKeyWrapper>()
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
                        new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
                        new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                        new OffensivePrefabKeyWrapper()
					};
				}
			}

            public static IList<IPrefabKeyWrapper> BoomAggressive
            {
                get
                {
                    List<IPrefabKeyWrapper> buildOrder = BoomCommon;

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
					List<IPrefabKeyWrapper> buildOrder = BoomCommon;

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

			private static List<IPrefabKeyWrapper> BoomCommon
            {
                get
                {
                    return new List<IPrefabKeyWrapper>()
                    {
                        new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
						new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
						new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                        new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
						new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.StealthGenerator),
						new AntiAirPrefabKeyWrapper(),
						new AntiNavalPrefabKeyWrapper(),
						new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
						new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
						new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
						new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
						new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
						new AntiAirPrefabKeyWrapper(),
                        new AntiNavalPrefabKeyWrapper()
                    };
                }
            }

            public static IList<IPrefabKeyWrapper> Rush
            {
                get
                {
                    return new List<IPrefabKeyWrapper>()
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
					};
                }
            }

			public static List<IPrefabKeyWrapper> Turtle
			{
				get
				{
					return new List<IPrefabKeyWrapper>()
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
                        new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
						new OffensivePrefabKeyWrapper(),
                        new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
						new OffensivePrefabKeyWrapper()
					};
				}
			}
        }
	}
}
