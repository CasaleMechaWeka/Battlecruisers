namespace BattleCruisers.Utils.Categorisation
{
    public interface IValueToStarConverterFactory
    {
        IValueToStarsConverter CreateUnitMovementSpeedConverter();

        // Health
        IValueToStarsConverter CreateBuildableHealthConverter();
        IValueToStarsConverter CreateCruiserHealthConverter();

        // Damage
        IValueToStarsConverter CreateAntiAirDamageConverter();
        IValueToStarsConverter CreateAntiShipDamageConverter();
        IValueToStarsConverter CreateAntiCruiserDamageConverter();
    }
}
