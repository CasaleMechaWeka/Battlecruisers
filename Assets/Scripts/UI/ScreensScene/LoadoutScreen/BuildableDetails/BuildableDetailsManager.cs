using BattleCruisers.Buildables;
using BattleCruisers.Fetchers;
using BattleCruisers.UI.Common.BuildingDetails;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.BuildableDetails
{
	public interface IBuildableDetailsManager
	{
		void SelectBuildable(BuildableLoadoutItem buildable);
		void CompareSelectedBuildable();
	}

	public class BuildableDetailsManager : MonoBehaviour, IBuildableDetailsManager, IPointerClickHandler
	{
		private IBuildableDetailsState _state;

		public Image modalBackground;
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

		public void SelectBuildable(BuildableLoadoutItem buildable)
		{
			_state = _state.SelectBuildable(buildable);
		}

		public void CompareSelectedBuildable()
		{
			_state = _state.CompareSelectedBuildable();
		}

		public void ShowBuildableDetails(Buildable buildable)
		{
			modalBackground.gameObject.SetActive(true);
			singleBuildableDetails.ShowItemDetails(buildable);
		}

		public void CompareBuildableDetails(Buildable buildable1, Buildable buildable2)
		{
			modalBackground.gameObject.SetActive(true);
			leftComparableBuildableDetails.ShowItemDetails(buildable1, buildable2);
			rightComparableBuildableDetails.ShowItemDetails(buildable2, buildable1);
		}

		public void HideBuildableDetails()
		{
			modalBackground.gameObject.SetActive(false);
			singleBuildableDetails.Hide();
			leftComparableBuildableDetails.Hide();
			rightComparableBuildableDetails.Hide();
		}

		void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
		{
			_state = _state.Dismiss();
		}
	}
}
