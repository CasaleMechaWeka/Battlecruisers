﻿using System.Collections.Generic;
using BattleCruisers.Data.Models.PrefabKeys;

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
						new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
						new OffensivePrefabKeyWrapper(),
						new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
						new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
						new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
						new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
						new OffensivePrefabKeyWrapper(),
						new OffensivePrefabKeyWrapper(),
						new OffensivePrefabKeyWrapper(),
						new OffensivePrefabKeyWrapper()
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
                        new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                        new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                        new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                        new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
						new OffensivePrefabKeyWrapper(),
						new OffensivePrefabKeyWrapper(),
						new OffensivePrefabKeyWrapper(),
						new OffensivePrefabKeyWrapper(),
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
						new OffensivePrefabKeyWrapper(),
						new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
						new OffensivePrefabKeyWrapper(),
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
						new OffensivePrefabKeyWrapper(),
						new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
						new AntiAirPrefabKeyWrapper(),
						new AntiNavalPrefabKeyWrapper(),
						new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
						new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
						new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
						new OffensivePrefabKeyWrapper(),
						new AntiAirPrefabKeyWrapper(),
						new AntiNavalPrefabKeyWrapper(),
						new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
						new AntiAirPrefabKeyWrapper(),
						new AntiNavalPrefabKeyWrapper(),
						new AntiAirPrefabKeyWrapper(),
						new AntiNavalPrefabKeyWrapper(),
						new OffensivePrefabKeyWrapper(),
						new AntiAirPrefabKeyWrapper(),
						new AntiNavalPrefabKeyWrapper(),
						new AntiAirPrefabKeyWrapper(),
						new OffensivePrefabKeyWrapper(),
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
                        new AntiNavalPrefabKeyWrapper(),
                        new OffensivePrefabKeyWrapper(),
                        new AntiAirPrefabKeyWrapper(),
                        new AntiNavalPrefabKeyWrapper(),
                        new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
                        new AntiAirPrefabKeyWrapper(),
                        new AntiNavalPrefabKeyWrapper(),
                        new AntiAirPrefabKeyWrapper(),
                        new AntiNavalPrefabKeyWrapper(),
                        new OffensivePrefabKeyWrapper(),
                        new AntiAirPrefabKeyWrapper(),
						new AntiNavalPrefabKeyWrapper(),
						new AntiAirPrefabKeyWrapper(),
						new OffensivePrefabKeyWrapper(),
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
                        new AntiAirPrefabKeyWrapper(),
						new AntiNavalPrefabKeyWrapper(),
						new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
						new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
						new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
						new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
						new AntiAirPrefabKeyWrapper(),
                        new AntiNavalPrefabKeyWrapper(),
                        new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.TeslaCoil),
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
						new OffensivePrefabKeyWrapper(),
						new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
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
						new AntiNavalPrefabKeyWrapper(),
						new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
						new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
						new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
						new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.TeslaCoil),
						new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
						new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
						new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.DroneStation),
                        new OffensivePrefabKeyWrapper(),
						new AntiAirPrefabKeyWrapper(),
						new AntiNavalPrefabKeyWrapper(),
						new OffensivePrefabKeyWrapper(),
						new AntiAirPrefabKeyWrapper(),
						new AntiNavalPrefabKeyWrapper(),
                        new StaticPrefabKeyWrapper(StaticPrefabKeys.Buildings.ShieldGenerator),
						new AntiAirPrefabKeyWrapper(),
						new AntiNavalPrefabKeyWrapper(),
						new AntiAirPrefabKeyWrapper(),
						new AntiNavalPrefabKeyWrapper(),
						new OffensivePrefabKeyWrapper(),
						new AntiAirPrefabKeyWrapper(),
						new OffensivePrefabKeyWrapper(),
						new OffensivePrefabKeyWrapper()
					};
				}
			}
        }

        // FELIX  Replace with DefensiveBuildOrderProvider :)
        public static IList<IPrefabKey> BasicAntiAir
        {
            get
            {
                return new List<IPrefabKey>()
                {
                    StaticPrefabKeys.Buildings.AntiAirTurret,
                    StaticPrefabKeys.Buildings.AntiAirTurret,
                    StaticPrefabKeys.Buildings.AntiAirTurret,
                    StaticPrefabKeys.Buildings.AntiAirTurret
                };
            }
        }

		public static IList<IPrefabKey> AntiAir
		{
			get
			{
				return new List<IPrefabKey>()
				{
					StaticPrefabKeys.Buildings.AntiAirTurret,
					StaticPrefabKeys.Buildings.SamSite,
					StaticPrefabKeys.Buildings.SamSite,
					StaticPrefabKeys.Buildings.SamSite
				};
			}
		}

		public static IList<IPrefabKey> BasicAntiNaval
		{
			get
			{
				return new List<IPrefabKey>()
				{
					StaticPrefabKeys.Buildings.AntiShipTurret,
					StaticPrefabKeys.Buildings.AntiShipTurret,
					StaticPrefabKeys.Buildings.AntiShipTurret,
					StaticPrefabKeys.Buildings.AntiShipTurret
				};
			}
		}

        public static IList<IPrefabKey> AntiNaval
        {
            get
            {
                return new List<IPrefabKey>()
                {
					StaticPrefabKeys.Buildings.AntiShipTurret,
					StaticPrefabKeys.Buildings.Mortar,
					StaticPrefabKeys.Buildings.Mortar,
					StaticPrefabKeys.Buildings.Mortar
                };
            }
        }

		public static IList<IPrefabKey> AntiRocketLauncher
		{
			get
			{
				return new List<IPrefabKey>()
				{
                    StaticPrefabKeys.Buildings.TeslaCoil
				};
			}
		}
	}
}
