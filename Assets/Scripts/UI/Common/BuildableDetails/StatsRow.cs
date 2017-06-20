using System;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildingDetails
{
	public abstract class StatsRow : MonoBehaviour
	{
		public Text rowLabel;
		public Image comparisonFeedbackBackground;

		private static Color BETTER_COLOR = Color.green;
		private static Color WORSE_COLOR = Color.red;

		public void Iniitalise(string statName)
		{
			rowLabel.text = statName;
		}
	}
}

