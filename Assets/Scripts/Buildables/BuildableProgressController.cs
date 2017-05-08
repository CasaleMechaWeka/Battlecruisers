using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Buildables
{
	public class BuildableProgressController : MonoBehaviour 
	{
		public Buildable buildable;
		public Image fillableImage;
		public Image outlineImage;

		void Awake() 
		{
			fillableImage.fillAmount = 0;
			gameObject.SetActive(false);

			buildable.StartedConstruction += Buildable_StartedBuilding;
			buildable.BuildableProgress += Buildable_BuildableProgress;
			buildable.CompletedBuildable += Buildable_CompletedOrDestroyedBuilding;
			buildable.Destroyed += Buildable_CompletedOrDestroyedBuilding;
		}

		private void Buildable_StartedBuilding(object sender, EventArgs e)
		{
			gameObject.SetActive(true);
		}
		
		private void Buildable_BuildableProgress(object sender, BuildProgressEventArgs e)
		{
			Debug.Log("e.Buildable.BuildProgress: " + e.Buildable.BuildProgress);

			Assert.IsTrue(e.Buildable.BuildProgress >= 0);
			fillableImage.fillAmount = e.Buildable.BuildProgress;
		}
		
		private void Buildable_CompletedOrDestroyedBuilding(object sender, EventArgs e)
		{
			buildable.StartedConstruction -= Buildable_StartedBuilding;
			buildable.BuildableProgress -= Buildable_BuildableProgress;
			buildable.CompletedBuildable -= Buildable_CompletedOrDestroyedBuilding;
			buildable.Destroyed -= Buildable_CompletedOrDestroyedBuilding;

			fillableImage.enabled = false;
			outlineImage.enabled = false;
		}
	}
}
