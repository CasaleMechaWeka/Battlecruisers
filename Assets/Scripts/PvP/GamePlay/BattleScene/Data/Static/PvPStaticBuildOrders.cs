using System.Collections.Generic;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys.Wrappers;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static
{
    public static class PvPStaticBuildOrders
    {
        public static class PvPAdaptive
        {
            public static IList<IPvPPrefabKeyWrapper> Balanced
            {
                get
                {
                    return new List<IPvPPrefabKeyWrapper>()
                    {
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPShieldGenerator),
                        new PvPOffensivePrefabKeyWrapper(),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPStealthGenerator),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPShieldGenerator),
                        new PvPOffensivePrefabKeyWrapper(),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPShieldGenerator),
                        new PvPOffensivePrefabKeyWrapper(),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPShieldGenerator),
                        new PvPOffensivePrefabKeyWrapper(),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPShieldGenerator),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation)
                    };
                }
            }

            public static IList<IPvPPrefabKeyWrapper> Boom
            {
                get
                {
                    return new List<IPvPPrefabKeyWrapper>()
                    {
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPShieldGenerator),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPStealthGenerator),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPShieldGenerator),
                        new PvPOffensivePrefabKeyWrapper(),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPShieldGenerator),
                        new PvPOffensivePrefabKeyWrapper(),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPShieldGenerator),
                        new PvPOffensivePrefabKeyWrapper(),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPShieldGenerator),
                        new PvPOffensivePrefabKeyWrapper()
                    };
                }
            }

            public static IList<IPvPPrefabKeyWrapper> Rush
            {
                get
                {
                    return new List<IPvPPrefabKeyWrapper>()
                    {
                        new PvPOffensivePrefabKeyWrapper(),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPOffensivePrefabKeyWrapper(),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPOffensivePrefabKeyWrapper(),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPOffensivePrefabKeyWrapper(),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation)
                    };
                }
            }
        }

        public static class Basic
        {
            public static IList<IPvPPrefabKeyWrapper> Balanced
            {
                get
                {
                    return new List<IPvPPrefabKeyWrapper>()
                    {
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPAntiAirPrefabKeyWrapper(),
                        new PvPAntiNavalPrefabKeyWrapper(),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPAntiAirPrefabKeyWrapper(),
                        new PvPAntiNavalPrefabKeyWrapper(),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPShieldGenerator),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPTeslaCoil),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPStealthGenerator),
                        new PvPOffensivePrefabKeyWrapper(),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPShieldGenerator),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPOffensivePrefabKeyWrapper(),
                        new PvPAntiAirPrefabKeyWrapper(),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPShieldGenerator),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPShieldGenerator),
                        new PvPOffensivePrefabKeyWrapper(),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPShieldGenerator),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPOffensivePrefabKeyWrapper()
                    };
                }
            }

            public static IList<IPvPPrefabKeyWrapper> BoomAggressive
            {
                get
                {
                    List<IPvPPrefabKeyWrapper> buildOrder = BoomCommon;

                    buildOrder.AddRange(new IPvPPrefabKeyWrapper[]
                    {
                        new PvPOffensivePrefabKeyWrapper(),
                        new PvPOffensivePrefabKeyWrapper(),
                        new PvPOffensivePrefabKeyWrapper(),
                        new PvPOffensivePrefabKeyWrapper()
                    });

                    return buildOrder;
                }
            }

            public static IList<IPvPPrefabKeyWrapper> BoomDefensive
            {
                get
                {
                    List<IPvPPrefabKeyWrapper> buildOrder = BoomCommon;

                    buildOrder.AddRange(new IPvPPrefabKeyWrapper[]
                    {
                        new PvPOffensivePrefabKeyWrapper(),
                        new PvPAntiAirPrefabKeyWrapper(),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPTeslaCoil),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPShieldGenerator),
                        new PvPOffensivePrefabKeyWrapper(),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPShieldGenerator),
                        new PvPOffensivePrefabKeyWrapper(),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPShieldGenerator),
                        new PvPOffensivePrefabKeyWrapper()
                    });

                    return buildOrder;
                }
            }

            private static List<IPvPPrefabKeyWrapper> BoomCommon
            {
                get
                {
                    return new List<IPvPPrefabKeyWrapper>()
                    {
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPShieldGenerator),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPStealthGenerator),
                        new PvPAntiAirPrefabKeyWrapper(),
                        new PvPAntiNavalPrefabKeyWrapper(),

                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPShieldGenerator),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPAntiAirPrefabKeyWrapper(),
                        new PvPAntiNavalPrefabKeyWrapper()
                    };
                }
            }

            public static IList<IPvPPrefabKeyWrapper> BOOM
            {
                get
                {
                    return new List<IPvPPrefabKeyWrapper>()
                    {
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation4),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation4),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPShieldGenerator),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPShieldGenerator),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPStealthGenerator),
                        new PvPOffensivePrefabKeyWrapper(),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation8),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation8),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPShieldGenerator),
                        new PvPOffensivePrefabKeyWrapper(),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPShieldGenerator),
                        new PvPOffensivePrefabKeyWrapper(),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPShieldGenerator),
                        new PvPOffensivePrefabKeyWrapper()
                    };
                }
            }

            public static IList<IPvPPrefabKeyWrapper> Rush
            {
                get
                {
                    return new List<IPvPPrefabKeyWrapper>()
                    {
                        new PvPOffensivePrefabKeyWrapper(),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPAntiAirPrefabKeyWrapper(),
                        new PvPAntiNavalPrefabKeyWrapper(),
                        new PvPOffensivePrefabKeyWrapper(),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPAntiAirPrefabKeyWrapper(),
                        new PvPAntiNavalPrefabKeyWrapper(),
                        new PvPOffensivePrefabKeyWrapper(),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPOffensivePrefabKeyWrapper(),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPOffensivePrefabKeyWrapper(),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation)
                    };
                }
            }

            public static List<IPvPPrefabKeyWrapper> Turtle
            {
                get
                {
                    return new List<IPvPPrefabKeyWrapper>()
                    {
                        new PvPAntiAirPrefabKeyWrapper(),
                        new PvPAntiNavalPrefabKeyWrapper(),
                        new PvPAntiAirPrefabKeyWrapper(),
                        new PvPAntiNavalPrefabKeyWrapper(),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPShieldGenerator),
                        new PvPAntiAirPrefabKeyWrapper(),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPShieldGenerator),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPStealthGenerator),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPTeslaCoil),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPOffensivePrefabKeyWrapper(),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPShieldGenerator),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPDroneStation),
                        new PvPOffensivePrefabKeyWrapper(),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPShieldGenerator),
                        new PvPOffensivePrefabKeyWrapper(),
                        new PvPStaticPrefabKeyWrapper(PvPStaticPrefabKeys.PvPBuildings.PvPShieldGenerator),
                        new PvPOffensivePrefabKeyWrapper()
                    };
                }
            }
        }
    }
}
