using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.DataStrctures;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Wind
{
    public class PvPProportionCalculator : IPvPProportionCalculator
    {
        public float FindProportion(float value, IPvPRange<float> range)
        {
            Assert.IsNotNull(range);

            value = Mathf.Clamp(value, range.Min, range.Max);

            float sizeAboveMin = value - range.Min;
            float rangeValue = range.Max - range.Min;

            return sizeAboveMin / rangeValue;
        }
    }
}