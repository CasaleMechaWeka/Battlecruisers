using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildingDetails
{
	public class StatsRowNumberController : StatsRow
	{
		public Text rowValue;

		public void Initialise(string label, string value, ComparisonResult comparisonResult)
		{
			base.Iniitalise(label, comparisonResult);
			rowValue.text = value;
		}
	}
}
