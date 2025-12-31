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
            public static OffensiveRequest[] NavalAirOffensiveUltra = new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low)
                };
            public static OffensiveRequest[] AirNavalOffensiveUltra = new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low)
                };
            public static OffensiveRequest[] NavalOffensiveAir = new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low)
                };
            public static OffensiveRequest[] AirOffensiveNaval = new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low)
                };
            public static OffensiveRequest[] NavalAirOffensive = new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)
                };
            public static OffensiveRequest[] AirNavalOffensive = new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)
                };
            public static IList<OffensiveRequest[]> All = new List<OffensiveRequest[]>()
            {
                NavalAirOffensiveUltra,
                AirNavalOffensiveUltra,
                NavalOffensiveAir,
                AirOffensiveNaval,
                AirNavalOffensive,
                NavalAirOffensive
            };
            public static IList<OffensiveRequest[]> NoUltras = new List<OffensiveRequest[]>()
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
            public static OffensiveRequest[] NavalOffensive = new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)
                };
            public static OffensiveRequest[] OffensiveUltraAir = new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.High),
                };
            public static OffensiveRequest[] OffensiveAirNaval = new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low)
                };
            public static OffensiveRequest[] OffensiveNavalAir = new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low)
                };
            public static OffensiveRequest[] OffensiveNaval = new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low)
                };
            public static OffensiveRequest[] OffensiveAirUltra = new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low)
                };
            public static OffensiveRequest[] OffensiveNavalOffensive = new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)
                };
            public static OffensiveRequest[] OffensiveAirOffensive = new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)
                };

            public static List<OffensiveRequest[]> All = new List<OffensiveRequest[]>()
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

            public static List<OffensiveRequest[]> NoUltras = new List<OffensiveRequest[]>()
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
            public static OffensiveRequest[] UltrasOffOff = new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low)

                };
            public static OffensiveRequest[] UltraNavalOffensive = new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)
                };
            public static OffensiveRequest[] UltraOffensiveUltraNaval = new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low)
                };
            public static OffensiveRequest[] UltraAirNaval = new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low)
                };
            public static OffensiveRequest[] UltraNavalUltra = new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High)
                };
            public static OffensiveRequest[] NavalOffensiveAir = new OffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low)
                };
            public static List<OffensiveRequest[]> All = new List<OffensiveRequest[]>()
            {
                 UltrasOffOff,
                 UltraNavalOffensive,
                 UltraOffensiveUltraNaval,
                 UltraAirNaval,
                 UltraNavalUltra,
                 NavalOffensiveAir
             };
            public static List<OffensiveRequest[]> NoUltras = new List<OffensiveRequest[]>()
            {
                 NavalOffensiveAir
            };
        }
    }
}