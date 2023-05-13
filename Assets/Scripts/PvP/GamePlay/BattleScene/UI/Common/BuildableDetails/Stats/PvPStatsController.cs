using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Categorisation;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails.Stats
{
    public abstract class PvPStatsController<T> : MonoBehaviour where T : class, IPvPTarget
    {
        protected IPvPValueToStarsConverter _unitMovementSpeedConverter;
        protected IPvPValueToStarsConverter _buildableHealthConverter, _cruiserHealthConverter;
        protected IPvPValueToStarsConverter _antiAirDamageConverter, _antiShipDamageConverter, _antiCruiserConverter;
        protected IPvPStatsComparer _higherIsBetterComparer, _lowerIsBetterComparer;

        public virtual void Initialise()
        {
            IPvPValueToStarsConverterFactory converterFactory = new PvPValueToStarsConverterFactory();

            _unitMovementSpeedConverter = converterFactory.CreateUnitMovementSpeedConverter();
            _buildableHealthConverter = converterFactory.CreateBuildableHealthConverter();
            _cruiserHealthConverter = converterFactory.CreateCruiserHealthConverter();
            _antiAirDamageConverter = converterFactory.CreateAntiAirDamageConverter();
            _antiShipDamageConverter = converterFactory.CreateAntiShipDamageConverter();
            _antiCruiserConverter = converterFactory.CreateAntiCruiserDamageConverter();

            _higherIsBetterComparer = new PvPHigherIsBetterComparer();
            _lowerIsBetterComparer = new PvPLowerIsBetterComparer();
        }

        public void ShowStats(T item, T itemToCompareTo = null)
        {
            if (itemToCompareTo == null)
            {
                itemToCompareTo = item;
            }

            InternalShowStats(item, itemToCompareTo);
        }

        protected abstract void InternalShowStats(T item, T itemToCompareTo);
    }
}