namespace BattleCruisers.Utils.Categorisation
{
        public class AntiCruiserDamageToStarsConverter : ValueToStarsConverter
        {
                private static readonly float[] CATEGORY_THRESHOLDS =
                {
            1,
                    // Bomber:      8.3 DPS
                    // MissilePod:  12.5 DPS
            15,
                    // SteamCopter: 17.8 DPS
                    // Frigate:     24 DPS (Mortar)
            30,
                    // Artillery:   30 DPS
                    // MLRS:        31.5 DPS
                    // LasCannon:   33.7 DPS
                    // IonCannon:   35.7 DPS
                    // Destroyer:   38 DPS (combined)
                    // AttackBoat:  40 DPS
            50,

                    // Nova:        50 DPS
                    // Orbital:     52.2 DPS
                    // MissileRev:  68 DPS
            70
                    // Mann o' War: 88.9 DPS
                    // Broadsides:  90 DPS
        };

                public AntiCruiserDamageToStarsConverter() : base(CATEGORY_THRESHOLDS)
                {
                }
        }
}
