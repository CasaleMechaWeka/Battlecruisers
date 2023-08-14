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
            public static IOffensiveRequest[] AirOffensive = new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)
                };
            public static IOffensiveRequest[] NavalOffensive = new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)
                };
            public static IList<IOffensiveRequest[]> All = new List<IOffensiveRequest[]>()
            {
                NavalAirOffensiveUltra,
                AirNavalOffensiveUltra,
                NavalOffensiveAir,
                AirOffensiveNaval,
                NavalOffensive,
                AirOffensive
            };
            public static IList<IOffensiveRequest[]> NoUltras = new List<IOffensiveRequest[]>()
            {
                NavalOffensiveAir,
                AirOffensiveNaval,
                NavalOffensive,
                AirOffensive
            };
        }

        /// <summary>
        /// Due to drone number limitations:
        /// + Cannot have Ultra as first request
        /// </summary>
        public class Balanced
        {
            public static IOffensiveRequest[] Offensive = new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)
                };
            public static IOffensiveRequest[] OffensiveUltra = new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High)
                };
            public static IOffensiveRequest[] OffensiveAirNaval = new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low)
                };
            public static IOffensiveRequest[] OffensiveNavalAir = new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low)
                };
            public static IOffensiveRequest[] OffensiveNaval = new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low)
                };
            public static IOffensiveRequest[] OffensiveAir = new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low)
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
            public static IList<IOffensiveRequest[]> All, NoUltras;

            static Balanced()
            {
                List<IOffensiveRequest[]> all = new List<IOffensiveRequest[]>()
                {
                    Offensive,
                    OffensiveUltra,
                    OffensiveAirNaval,
                    OffensiveNavalAir,
                    OffensiveNaval,
                    OffensiveAir,
                    OffensiveNavalOffensive,
                    OffensiveAirOffensive,
                };
                all.AddRange(Rush.All);
                All = all;

                List<IOffensiveRequest[]> noUltras = new List<IOffensiveRequest[]>()
                {
                    Offensive,
                    OffensiveAirNaval,
                    OffensiveNavalAir,
                    OffensiveNaval,
                    OffensiveAir,
                    OffensiveNavalOffensive,
                    OffensiveAirOffensive,
                };
                noUltras.AddRange(Rush.NoUltras);
                NoUltras = noUltras;
            }
        }

        /// <summary>
        /// Have at least 10 drones at the first offensive request, so can afford 
        /// all offensive request types.
        /// </summary>
        public class Boom
        {
            public static IOffensiveRequest[] Ultras = new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High)
                };
            public static IOffensiveRequest[] UltraOffensive = new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.High)
                };
            public static IOffensiveRequest[] UltraOffensiveUltra = new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Buildings, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.High)
                };
            public static IOffensiveRequest[] UltraAir = new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Air, OffensiveFocus.Low)
                };
            public static IOffensiveRequest[] UltraNaval = new IOffensiveRequest[]
                {
                    new OffensiveRequest(OffensiveType.Ultras, OffensiveFocus.Low),
                    new OffensiveRequest(OffensiveType.Naval, OffensiveFocus.Low)
                };
            public static IList<IOffensiveRequest[]> All, NoUltras;

            static Boom()
            {
                List<IOffensiveRequest[]> all = new List<IOffensiveRequest[]>()
                {
                    Ultras,
                    UltraOffensive,
                    UltraOffensiveUltra,
                    UltraAir,
                    UltraNaval
                };
                all.AddRange(Balanced.All);
                All = all;

                NoUltras = Balanced.NoUltras;
            }
        }
    }
}