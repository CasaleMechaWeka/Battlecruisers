using System.Collections.Generic;

namespace BattleCruisers.Data.Static.Strategies.Requests
{
    public static class OffensiveRequestsProvider
    {
        /// <summary>
        /// Due to drone number limitations:
        /// + Cannot have Offensive as first request
        /// + Cannot have Ultra as first 3 requests
        /// </summary>
        public static class Rush
        {
            public static IOffensiveRequest[] NavalAirOffensiveUltra = new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low)
                };
            public static IOffensiveRequest[] AirNavalOffensiveUltra = new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low)
                };
            public static IOffensiveRequest[] NavalOffensiveAir = new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low)
                };
            public static IOffensiveRequest[] AirOffensiveNaval = new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low)
                };
            public static IOffensiveRequest[] NavalAirOffensive = new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)
                };
            public static IOffensiveRequest[] AirNavalOffensive = new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)
                };
            public static IList<IOffensiveRequest[]> All = new List<IOffensiveRequest[]>()
            {
                NavalAirOffensiveUltra,
                AirNavalOffensiveUltra,
                NavalOffensiveAir,
                AirOffensiveNaval,
                AirNavalOffensive,
                NavalAirOffensive
            };
            public static IList<IOffensiveRequest[]> NoUltras = new List<IOffensiveRequest[]>()
            {   
                NavalOffensiveAir,
                AirOffensiveNaval,
                AirNavalOffensive,
                NavalAirOffensive
            };
        }

        /// <summary>
        /// Due to drone number limitations:
        /// + Cannot have Ultra as first request
        /// </summary>
        public static class Balanced
        {
            public static IOffensiveRequest[] NavalOffensive = new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)
                };
            public static IOffensiveRequest[] OffensiveUltraAir = new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.High),
                };
            public static IOffensiveRequest[] OffensiveAirNaval = new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low)
                };
            public static IOffensiveRequest[] OffensiveNavalAir = new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low)
                };
            public static IOffensiveRequest[] OffensiveNaval = new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low)
                };
            public static IOffensiveRequest[] OffensiveAirUltra = new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low)
                };
            public static IOffensiveRequest[] OffensiveNavalOffensive = new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)
                };
            public static IOffensiveRequest[] OffensiveAirOffensive = new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)
                };

            public static List<IOffensiveRequest[]> All = new List<IOffensiveRequest[]>()
            {
                NavalOffensive,
                OffensiveUltraAir,
                OffensiveAirNaval,
                OffensiveNavalAir,
                OffensiveNaval,
                OffensiveAirUltra,
                OffensiveNavalOffensive,
                OffensiveAirOffensive,
            };

            public static List<IOffensiveRequest[]> NoUltras = new List<IOffensiveRequest[]>()
            {
                NavalOffensive,
                OffensiveAirNaval,
                OffensiveNavalAir,
                OffensiveNaval,
                OffensiveNavalOffensive,
                OffensiveAirOffensive,
            };
        }

        /// <summary>
        /// Have at least 10 drones at the first offensive request, so can afford 
        /// all offensive request types.
        /// </summary>
        public static class Boom
        {
            public static IOffensiveRequest[] UltrasOffOff = new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low)

                };
            public static IOffensiveRequest[] UltraNavalOffensive = new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)
                };
            public static IOffensiveRequest[] UltraOffensiveUltraNaval = new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low)
                };
            public static IOffensiveRequest[] UltraAirNaval = new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low)
                };
            public static IOffensiveRequest[] UltraNavalUltra = new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High)
                };
            public static IOffensiveRequest[] NavalOffensiveAir = new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low)
                };
            public static List<IOffensiveRequest[]> All = new List<IOffensiveRequest[]>()
            {
                 UltrasOffOff,
                 UltraNavalOffensive,
                 UltraOffensiveUltraNaval,
                 UltraAirNaval,
                 UltraNavalUltra,
                 NavalOffensiveAir
             };
            public static List<IOffensiveRequest[]> NoUltras = new List<IOffensiveRequest[]>()
            {
                 NavalOffensiveAir
            };
        }
    }
}