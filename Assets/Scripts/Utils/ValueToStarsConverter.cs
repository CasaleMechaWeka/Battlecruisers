using System;
using System.Collections.Generic;

namespace BattleCruisers.Utils.Categorisation
{
    public enum ValueType
    {
        MovementSpeed = 0,
        BuildableHealth = 1,
        CruiserHealth = 2,
        AntiAir = 3,
        AntiShip = 4,
        AntiCruiser = 5
    }

    public static class ValueToStarsConverter
    {
        private static readonly IReadOnlyList<float[]> Thresholds = new[]
        {
            new float[] { 1.5f, 2, 4, 6, 8 },           // UnitMovementSpeed
            new float[] { 1, 200, 400, 600, 1000 },     // BuildableHealth
            new float[] { 1, 2400, 3600, 4800, 6000 },  // CruiserHealth
            new float[] { 1, 10, 23, 27, 50 },          // AntiAirDamage
            new float[] { 1, 20, 40, 60, 80 },          // AntiShipDamage
            new float[] { 1, 15, 30, 50, 70 }           // AntiCruiserDamage
        };

        public static int ConvertValueToStars(float value, ValueType valueType)
        {
            float[] thresholds = Thresholds[(int)valueType];
            int index = Array.BinarySearch(thresholds, value);
            return index >= 0 ? index + 1 : ~index;
        }
    }
}
