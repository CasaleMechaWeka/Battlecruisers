namespace BattleCruisers.Utils.Categorisation
{
    // FELIX  Rename to stars :P
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
