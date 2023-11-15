namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Categorisation
{
    public interface IPvPValueToStarsConverterFactory
    {
        IPvPValueToStarsConverter CreateUnitMovementSpeedConverter();

        // Health
        IPvPValueToStarsConverter CreateBuildableHealthConverter();
        IPvPValueToStarsConverter CreateCruiserHealthConverter();

        // Damage
        IPvPValueToStarsConverter CreateAntiAirDamageConverter();
        IPvPValueToStarsConverter CreateAntiShipDamageConverter();
        IPvPValueToStarsConverter CreateAntiCruiserDamageConverter();
    }
}
