using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers
{
	public class BuildableProgressController : MonoBehaviour 
	{
		public Image image;
		public Buildable buildable;

		void Awake() 
		{
			image.fillAmount = 0;
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
			Assert.IsTrue(e.BuildProgress >= 0);
			image.fillAmount = e.BuildProgress;
		}
		
		private void Buildable_CompletedOrDestroyedBuilding(object sender, EventArgs e)
		{
			buildable.StartedConstruction -= Buildable_StartedBuilding;
			buildable.BuildableProgress -= Buildable_BuildableProgress;
			buildable.CompletedBuildable -= Buildable_CompletedOrDestroyedBuilding;
			buildable.Destroyed -= Buildable_CompletedOrDestroyedBuilding;
			
			Destroy(gameObject);
		}
	}
}
