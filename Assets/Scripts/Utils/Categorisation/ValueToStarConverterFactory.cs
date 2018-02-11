namespace BattleCruisers.Utils.Categorisation
{
    public class ValueToStarConverterFactory : IValueToStarConverterFactory
    {
        public IValueToStarsConverter CreateUnitMovementSpeedConverter()
        {
            return new UnitMovementSpeedToStarsConverter();
        }

        #region Health
        public IValueToStarsConverter CreateBuildableHealthConverter()
        {
            return new BuildableHealthToStarsConverter();
        }

        public IValueToStarsConverter CreateCruiserHealthConverter()
        {
            return new CruiserHealthToStarsConverter();
        }
        #endregion Health

        #region Damage
        public IValueToStarsConverter CreateAntiAirDamageConverter()
        {
            return new AntiAirDamageToStarsConverter();
        }
		
		public IValueToStarsConverter CreateAntiShipDamageConverter()
		{
            return new AntiShipDamageToStarsConverter();
		}

        public IValueToStarsConverter CreateAntiCruiserDamageConverter()
        {
            return new AntiCruiserDamageToStarsConverter();
        }
        #endregion Damage
    }
}
