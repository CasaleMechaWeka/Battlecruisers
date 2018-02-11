namespace BattleCruisers.Utils.Categorisation
{
    public interface IValueToStarsConverterFactory
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
