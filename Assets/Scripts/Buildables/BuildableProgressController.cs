using System;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Buildables
{
    public class BuildableProgressController : MonoBehaviour
	{
		private Buildable _buildable;
		private Image _fillableImage;
		private Image _outlineImage;

		public Sprite FillableImageSprite { get { return _fillableImage.sprite; } }

		public void Initialise() 
		{
			_buildable = gameObject.GetComponentInInactiveParent<Buildable>();
			Assert.IsNotNull(_buildable);

			GameObject fillableImageGameObject = transform.Find("Canvas/FillableImage").gameObject;
			Assert.IsNotNull(fillableImageGameObject);
			_fillableImage = fillableImageGameObject.GetComponent<Image>();
			Assert.IsNotNull(_fillableImage);

			GameObject outlineImageGameObject = transform.Find("Canvas/OutlineImage").gameObject;
			Assert.IsNotNull(outlineImageGameObject);
			_outlineImage = outlineImageGameObject.GetComponent<Image>();
			Assert.IsNotNull(_outlineImage);

			_fillableImage.fillAmount = 0;
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
			Logging.Log(Tags.PROGRESS_BARS, "e.Buildable.BuildProgress: " + e.Buildable.BuildProgress);

			Assert.IsTrue(e.Buildable.BuildProgress >= 0);
			_fillableImage.fillAmount = e.Buildable.BuildProgress;
		}
		
		private void Buildable_CompletedOrDestroyedBuilding(object sender, EventArgs e)
		{
			_buildable.StartedConstruction -= Buildable_StartedBuilding;
			_buildable.BuildableProgress -= Buildable_BuildableProgress;
			_buildable.CompletedBuildable -= Buildable_CompletedOrDestroyedBuilding;
			_buildable.Destroyed -= Buildable_CompletedOrDestroyedBuilding;

			_fillableImage.enabled = false;
			_outlineImage.enabled = false;
		}
	}
}
