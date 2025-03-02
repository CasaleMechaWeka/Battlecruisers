using BattleCruisers.Utils.Categorisation;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Categorisation
{
    public class PvPValueToStarsConverterFactory : IPvPValueToStarsConverterFactory
    {
        public IValueToStarsConverter CreateUnitMovementSpeedConverter()
        {
            return new PvPUnitMovementSpeedToStarsConverter();
        }

        #region Health
        public IValueToStarsConverter CreateBuildableHealthConverter()
        {
            return new PvPBuildableHealthToStarsConverter();
        }

        public IValueToStarsConverter CreateCruiserHealthConverter()
        {
            return new PvPCruiserHealthToStarsConverter();
        }
        #endregion Health

        #region Damage
        public IValueToStarsConverter CreateAntiAirDamageConverter()
        {
            return new PvPAntiAirDamageToStarsConverter();
        }

        public IValueToStarsConverter CreateAntiShipDamageConverter()
        {
            return new PvPAntiShipDamageToStarsConverter();
        }

        public IValueToStarsConverter CreateAntiCruiserDamageConverter()
        {
            return new PvPAntiCruiserDamageToStarsConverter();
        }
        #endregion Damage
    }
}
