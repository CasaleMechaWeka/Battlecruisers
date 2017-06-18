using BattleCruisers.Buildables;
using BattleCruisers.UI.Common.BuildingDetails;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
	public interface IBuildableDetailsManager
	{
		void ShowSingleBuildable(Buildable buildable);
		void Hide();
	}

	public class BuildableDetailsManager : MonoBehaviour
	{
		public ComparableBuildableDetailsController singleBuildableDetails;
		public ComparableBuildableDetailsController leftComparableBuildableDetails, rightComparableBuildableDetails;

		void Start()
		{
			Hide();
		}

		public void ShowSingleBuildable(Buildable buildable)
		{
			singleBuildableDetails.ShowBuildableDetails(buildable);
		}

		public void Hide()
		{
			singleBuildableDetails.Hide();
			// FELIX
//			leftComparableBuildableDetails.Hide();
//			rightComparableBuildableDetails.Hide();
		}
	}
}
