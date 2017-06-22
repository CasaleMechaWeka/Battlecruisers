using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildingDetails.Stats
{
	public class StatsRowNumberController : StatsRow
	{
		public Text rowValue;
		
		public void Initialise(string label, int value, ComparisonResult comparisonResult)
		{
			Initialise(label, value.ToString(), comparisonResult);
		}

		public void Initialise(string label, float value, ComparisonResult comparisonResult)
		{
			Initialise(label, value.ToString(), comparisonResult);
		}

		public void Initialise(string label, string value, ComparisonResult comparisonResult)
		{
			base.Iniitalise(label, comparisonResult);
			rowValue.text = value;
		}
	}
}
