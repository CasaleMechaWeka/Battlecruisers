using System;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildingDetails
{
	public abstract class StatsRow : MonoBehaviour
	{
		public Text rowLabel;
		public Image comparisonFeedbackBackground;

		public void Iniitalise(string statName, ComparisonResult comparisonResult)
		{
			rowLabel.text = statName;
			comparisonFeedbackBackground.color = comparisonResult.Color;
		}
	}
}

