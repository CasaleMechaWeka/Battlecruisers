using BattleCruisers.Utils.Categorisation;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Categorisation
{
    public interface IPvPValueToStarsConverterFactory
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
