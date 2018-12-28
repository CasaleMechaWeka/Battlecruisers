using UnityEngine.Assertions;

namespace BattleCruisers.UI.Common.BuildableDetails.Stats
{
    public class StatsRowStarsController : StatsRow
	{
        private StarController[] _stars;

		private const int MIN_RATING = 0;
		private const int MAX_RATING = 5;

        public override void Initialise()
        {
            base.Initialise();

            _stars = GetComponentsInChildren<StarController>();
            Assert.AreEqual(MAX_RATING, _stars.Length);

            foreach (StarController star in _stars)
            {
                star.Initialise();
            }
        }

		public void ShowResult(int statRating, ComparisonResult comparisonResult)
		{
			base.ShowResult(comparisonResult);

			for (int i = 0; i < _stars.Length; ++i)
			{
                StarController star = _stars[i];
                star.Enabled = i < statRating;
			}
		}
	}
}
