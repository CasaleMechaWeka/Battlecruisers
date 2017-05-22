using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.BuildingDetails
{
	public class StatsRowNumberController : MonoBehaviour 
	{
		public Text rowLabel;
		public Text rowValue;

		public void Initialise(string label, string value)
		{
			rowLabel.text = label;
			rowValue.text = value;
		}
	}
}
