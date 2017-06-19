using BattleCruisers.Buildables;
using BattleCruisers.Fetchers;
using BattleCruisers.UI.Common.BuildingDetails;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

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
		private Buildable _selectedBuildable;

		public ComparableBuildableDetailsController singleBuildableDetails;
		public ComparableBuildableDetailsController leftComparableBuildableDetails, rightComparableBuildableDetails;

		void Start()
		{
			SpriteFetcher spriteFetcher = new SpriteFetcher();

			singleBuildableDetails.Initialise(spriteFetcher);
			leftComparableBuildableDetails.Initialise(spriteFetcher);
			rightComparableBuildableDetails.Initialise(spriteFetcher);

			InternalDismiss();
		}

		public void SelectBuildable(Buildable buildable)
		{
			if (_selectedBuildable == null)
			{
				_selectedBuildable = buildable;
				singleBuildableDetails.ShowBuildableDetails(_selectedBuildable);
			}
			else
			{
				leftComparableBuildableDetails.ShowBuildableDetails(_selectedBuildable);
				rightComparableBuildableDetails.ShowBuildableDetails(buildable);
			}
		}

		public void CompareSelectedBuildable()
		{
			InternalDismiss();
		}

		public void Dismiss()
		{
			Assert.IsNotNull(_selectedBuildable);
			_selectedBuildable = null;
			InternalDismiss();
		}

		private void InternalDismiss()
		{
			singleBuildableDetails.Hide();
			leftComparableBuildableDetails.Hide();
			rightComparableBuildableDetails.Hide();
		}
	}
}
