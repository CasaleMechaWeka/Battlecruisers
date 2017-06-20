using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildingDetails
{
	public class StatsRowStarsController : StatsRow
	{
		public Image[] stars;

		private const int MIN_RATING = 0;
		private const int MAX_RATING = 5;

		public void Initialise(string statName, int statRating, ComparisonResult comparisonResult)
		{
			base.Iniitalise(statName, comparisonResult);

			Assert.IsTrue(stars.Length == MAX_RATING);

			for (int i = 0; i < stars.Length; ++i)
			{
				Image star = stars[i];
				star.gameObject.SetActive(i < statRating);
			}
		}
	}
}
