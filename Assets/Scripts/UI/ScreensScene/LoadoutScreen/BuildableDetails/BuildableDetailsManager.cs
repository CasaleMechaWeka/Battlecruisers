using BattleCruisers.Buildables;
using BattleCruisers.Fetchers;
using BattleCruisers.UI.Common.BuildingDetails;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.BuildableDetails
{
	public interface IBuildableDetailsState
	{
		IBuildableDetailsState SelectBuildable(Buildable selectedBuildable);
		IBuildableDetailsState CompareSelectedBuildable();
		IBuildableDetailsState Dismiss();
	}

	public interface IBuildableDetailsManager
	{
		void SelectBuildable(Buildable buildable);
		void CompareSelectedBuildable();
		void Dismiss();
	}

	public class BuildableDetailsManager : MonoBehaviour, IBuildableDetailsManager
	{
		private IBuildableDetailsState _state;

		public ComparableBuildableDetailsController singleBuildableDetails;
		public ComparableBuildableDetailsController leftComparableBuildableDetails, rightComparableBuildableDetails;

		void Start()
		{
			SpriteFetcher spriteFetcher = new SpriteFetcher();

			singleBuildableDetails.Initialise(spriteFetcher);
			leftComparableBuildableDetails.Initialise(spriteFetcher);
			rightComparableBuildableDetails.Initialise(spriteFetcher);

			_state = new DismissedState(this);
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
			singleBuildableDetails.ShowBuildableDetails(buildable);
		}

		public void CompareBuildableDetails(Buildable buildable1, Buildable buildable2)
		{
			leftComparableBuildableDetails.ShowBuildableDetails(buildable1);
			rightComparableBuildableDetails.ShowBuildableDetails(buildable2);
		}

		public void HideBuildableDetails()
		{
			singleBuildableDetails.Hide();
			leftComparableBuildableDetails.Hide();
			rightComparableBuildableDetails.Hide();
		}
	}
}
