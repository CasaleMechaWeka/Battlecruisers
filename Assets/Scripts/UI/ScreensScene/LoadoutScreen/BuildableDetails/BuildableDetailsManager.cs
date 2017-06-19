using BattleCruisers.Buildables;
using BattleCruisers.Fetchers;
using BattleCruisers.UI.Common.BuildingDetails;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.BuildableDetails
{
	public interface IBuildableDetailsManager
	{
		void SelectBuildable(Buildable buildable);
		void CompareSelectedBuildable();
		void Dismiss();
	}

	public class BuildableDetailsManager : MonoBehaviour, IBuildableDetailsManager
	{
		private IBuildableDetailsState _state;

		public Image modelBackground;
		public ComparableBuildableDetailsController singleBuildableDetails;
		public ComparableBuildableDetailsController leftComparableBuildableDetails, rightComparableBuildableDetails;

		void Start()
		{
			SpriteFetcher spriteFetcher = new SpriteFetcher();

			singleBuildableDetails.Initialise(spriteFetcher);
			leftComparableBuildableDetails.Initialise(spriteFetcher);
			rightComparableBuildableDetails.Initialise(spriteFetcher);

			_state = new DismissedState(this);

			HideBuildableDetails();
		}

		public void SelectBuildable(Buildable buildable)
		{
			_state = _state.SelectBuildable(buildable);
		}

		public void CompareSelectedBuildable()
		{
			_state = _state.CompareSelectedBuildable();
		}

		public void Dismiss()
		{
			_state = _state.Dismiss();
		}

		public void ShowBuildableDetails(Buildable buildable)
		{
			modelBackground.gameObject.SetActive(true);
			singleBuildableDetails.ShowBuildableDetails(buildable);
		}

		public void CompareBuildableDetails(Buildable buildable1, Buildable buildable2)
		{
			modelBackground.gameObject.SetActive(true);
			leftComparableBuildableDetails.ShowBuildableDetails(buildable1);
			rightComparableBuildableDetails.ShowBuildableDetails(buildable2);
		}

		public void HideBuildableDetails()
		{
			modelBackground.gameObject.SetActive(false);
			singleBuildableDetails.Hide();
			leftComparableBuildableDetails.Hide();
			rightComparableBuildableDetails.Hide();
		}
	}
}
