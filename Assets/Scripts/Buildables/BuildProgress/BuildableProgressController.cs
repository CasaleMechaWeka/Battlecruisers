using System;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Buildables.BuildProgress
{
    public class BuildableProgressController : MonoBehaviour
	{
		private IBuildable _buildable;

		public Image FillableImage { get; private set; }
        public Image OutlineImage { get; private set; }

		public void Initialise() 
		{
			_buildable = gameObject.GetComponentInInactiveParent<IBuildable>();
			Assert.IsNotNull(_buildable);

            FillableImage = transform.FindNamedComponent<Image>("Canvas/FillableImage");
            OutlineImage = transform.FindNamedComponent<Image>("Canvas/OutlineImage");

			FillableImage.fillAmount = 0;
			gameObject.SetActive(false);

			_buildable.StartedConstruction += Buildable_StartedBuilding;
			_buildable.BuildableProgress += Buildable_BuildableProgress;
			_buildable.CompletedBuildable += Buildable_CompletedOrDestroyedBuilding;
			_buildable.Destroyed += Buildable_CompletedOrDestroyedBuilding;
		}

		private void Buildable_StartedBuilding(object sender, EventArgs e)
		{
			gameObject.SetActive(true);
		}
		
		private void Buildable_BuildableProgress(object sender, BuildProgressEventArgs e)
		{
			Logging.Verbose(Tags.PROGRESS_BARS, "e.Buildable.BuildProgress: " + e.Buildable.BuildProgress);

			Assert.IsTrue(e.Buildable.BuildProgress >= 0);
			FillableImage.fillAmount = e.Buildable.BuildProgress;
		}
		
		private void Buildable_CompletedOrDestroyedBuilding(object sender, EventArgs e)
		{
			_buildable.StartedConstruction -= Buildable_StartedBuilding;
			_buildable.BuildableProgress -= Buildable_BuildableProgress;
			_buildable.CompletedBuildable -= Buildable_CompletedOrDestroyedBuilding;
			_buildable.Destroyed -= Buildable_CompletedOrDestroyedBuilding;

			FillableImage.enabled = false;
			OutlineImage.enabled = false;
		}
	}
}
