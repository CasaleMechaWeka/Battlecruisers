namespace BattleCruisers.Utils.Categorisation
{
    public static class ValueToStarsConverterFactory
    {
        public static IValueToStarsConverter CreateUnitMovementSpeedConverter()
        {
            return new UnitMovementSpeedToStarsConverter();
        }

        #region Health
        public static IValueToStarsConverter CreateBuildableHealthConverter()
        {
            return new BuildableHealthToStarsConverter();
        }

        public static IValueToStarsConverter CreateCruiserHealthConverter()
        {
            return new CruiserHealthToStarsConverter();
        }
        #endregion Health

        #region Damage
        public static IValueToStarsConverter CreateAntiAirDamageConverter()
        {
            return new AntiAirDamageToStarsConverter();
        }

        public static IValueToStarsConverter CreateAntiShipDamageConverter()
        {
            return new AntiShipDamageToStarsConverter();
        }

        public static IValueToStarsConverter CreateAntiCruiserDamageConverter()
        {
            return new AntiCruiserDamageToStarsConverter();
        }
        #endregion Damage
    }
}
