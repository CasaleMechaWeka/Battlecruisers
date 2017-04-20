using BattleCruisers.Buildables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ProgressBars
{
	public abstract class BaseProgressBarController : MonoBehaviour
	{
		private float _outlineWidth;

		public Image progressBarOutline;
		public Image progressSoFar;
		public bool hideWhenFull;
		public float originalProgress;

		private bool AreImagesEnabled
		{
			get
			{
				return progressBarOutline.enabled && progressSoFar.enabled;
			}
		}

		void Awake()
		{
			_outlineWidth = ((RectTransform)progressBarOutline.transform).rect.width;
			OnProgressChanged(originalProgress);
		}

		protected void OnProgressChanged(float newProgress)
		{
			// FELIX
			if (newProgress < 0 || newProgress > 1)
			{
				int crapsies = 12;
			}

			Assert.IsTrue(newProgress >= 0 && newProgress <= 1);

			if (newProgress == 0)
			{
				OnProgressHitsZero();
			}
			else if (newProgress == 1)
			{
				OnProgressHitsOne();
			}
			else if(!AreImagesEnabled)
			{
				ShowProgressBar();
			}

			RectTransform newProgressRect = (RectTransform)progressSoFar.transform;
			newProgressRect.sizeDelta = new Vector2(newProgress * _outlineWidth, newProgressRect.sizeDelta.y);
		}

		protected virtual void OnProgressHitsZero()
		{
			HideProgressBar();
		}

		protected virtual void OnProgressHitsOne()
		{
			if (hideWhenFull)
			{
				HideProgressBar();
			}
		}

		private void ShowProgressBar()
		{
			EnableImages(true);
		}

		private void HideProgressBar()
		{
			EnableImages(false);
		}

		private void EnableImages(bool enabled)
		{
			progressBarOutline.enabled = enabled;
			progressSoFar.enabled = enabled;
		}
	}
}
