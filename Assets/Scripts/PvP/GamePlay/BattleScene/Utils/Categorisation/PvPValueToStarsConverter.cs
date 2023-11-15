using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Categorisation
{
    public abstract class PvPValueToStarsConverter : IPvPValueToStarsConverter
    {
        private readonly float[] _categoryThresholds;

        private const int MIN_NUM_OF_STARS = 0;
        private const int MAX_NUM_OF_STARS = 5;

        protected PvPValueToStarsConverter(float[] categoryThresholds)
        {
            Assert.IsNotNull(categoryThresholds);
            Assert.IsTrue(categoryThresholds.Length == MAX_NUM_OF_STARS);

            _categoryThresholds = categoryThresholds;
        }

        public int ConvertValueToStars(float value)
        {
            for (int i = _categoryThresholds.Length - 1; i >= 0; --i)
            {
                if (value >= _categoryThresholds[i])
                {
                    return i + 1;
                }
            }
            return MIN_NUM_OF_STARS;
        }
    }
}
