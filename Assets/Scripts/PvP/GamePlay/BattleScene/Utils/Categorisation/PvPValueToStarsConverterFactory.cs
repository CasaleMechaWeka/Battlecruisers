namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Categorisation
{
    public class PvPValueToStarsConverterFactory : IPvPValueToStarsConverterFactory
    {
        public IPvPValueToStarsConverter CreateUnitMovementSpeedConverter()
        {
            return new PvPUnitMovementSpeedToStarsConverter();
        }

        #region Health
        public IPvPValueToStarsConverter CreateBuildableHealthConverter()
        {
            return new PvPBuildableHealthToStarsConverter();
        }

        public IPvPValueToStarsConverter CreateCruiserHealthConverter()
        {
            return new PvPCruiserHealthToStarsConverter();
        }
        #endregion Health

        #region Damage
        public IPvPValueToStarsConverter CreateAntiAirDamageConverter()
        {
            return new PvPAntiAirDamageToStarsConverter();
        }

        public IPvPValueToStarsConverter CreateAntiShipDamageConverter()
        {
            return new PvPAntiShipDamageToStarsConverter();
        }

        public IPvPValueToStarsConverter CreateAntiCruiserDamageConverter()
        {
            return new PvPAntiCruiserDamageToStarsConverter();
        }
        #endregion Damage
    }
}
