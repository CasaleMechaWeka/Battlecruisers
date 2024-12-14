using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys.Wrappers;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static
{
    public static class PvPStaticBuildOrders
    {
        public static class PvPAdaptive
        {
            public static ReadOnlyCollection<IPvPPrefabKeyWrapper> Balanced = new ReadOnlyCollection<IPvPPrefabKeyWrapper>(new List<IPvPPrefabKeyWrapper>()
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
            });

            public static ReadOnlyCollection<IPvPPrefabKeyWrapper> Boom = new ReadOnlyCollection<IPvPPrefabKeyWrapper>(new List<IPvPPrefabKeyWrapper>()
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
            });

            public static ReadOnlyCollection<IPvPPrefabKeyWrapper> Rush = new ReadOnlyCollection<IPvPPrefabKeyWrapper>(new List<IPvPPrefabKeyWrapper>()
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
            });
        }

        public static class Basic
        {
            public static ReadOnlyCollection<IPvPPrefabKeyWrapper> Balanced = new ReadOnlyCollection<IPvPPrefabKeyWrapper>(new List<IPvPPrefabKeyWrapper>()
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
            });

            public static IList<IPvPPrefabKeyWrapper> BoomAggressive
            {
                get
                {
                    List<IPvPPrefabKeyWrapper> buildOrder = BoomCommon.ToList();

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
                    List<IPvPPrefabKeyWrapper> buildOrder = BoomCommon.ToList();

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

            public static ReadOnlyCollection<IPvPPrefabKeyWrapper> BoomCommon = new ReadOnlyCollection<IPvPPrefabKeyWrapper>(new List<IPvPPrefabKeyWrapper>()
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
            });

            public static ReadOnlyCollection<IPvPPrefabKeyWrapper> BOOM = new ReadOnlyCollection<IPvPPrefabKeyWrapper>(new List<IPvPPrefabKeyWrapper>()
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
            });

            public static ReadOnlyCollection<IPvPPrefabKeyWrapper> Rush = new ReadOnlyCollection<IPvPPrefabKeyWrapper>(new List<IPvPPrefabKeyWrapper>()
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
            });

            public static ReadOnlyCollection<IPvPPrefabKeyWrapper> Turtle = new ReadOnlyCollection<IPvPPrefabKeyWrapper>(new List<IPvPPrefabKeyWrapper>()
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
            });
        }
    }
}
