using System;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildingDetails
{
	// FELIX  Move to own class
	public interface IComparisonResult
	{
		Color Color { get; }
	}

	public abstract class ComparisonResult : IComparisonResult
	{
		protected static Color NEUTRAL_COLOR = Color.clear;
		protected static Color BETTER_COLOR = Color.green;
		protected static Color WORSE_COLOR = Color.red;

		public Color Color { get; private set; }

		protected ComparisonResult(Color color)
		{
			Color = color;
		}
	}

	public class NeutralResult : ComparisonResult
	{
		public NeutralResult() : base(NEUTRAL_COLOR) { }
	}

	public class BetterResult : ComparisonResult
	{
		public BetterResult() : base(BETTER_COLOR) { }
	}

	public class WorseResult : ComparisonResult
	{
		public WorseResult() : base(WORSE_COLOR) { }
	}

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

