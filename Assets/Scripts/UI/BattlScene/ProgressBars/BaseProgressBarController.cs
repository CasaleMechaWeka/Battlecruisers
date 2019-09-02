using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.ProgressBars
{
    public abstract class BaseProgressBarController : MonoBehaviourWrapper
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
				return 
                    progressBarOutline.enabled 
                    && progressSoFar.enabled;
			}
		}

		void Awake()
		{
			_outlineWidth = ((RectTransform)progressBarOutline.transform).rect.width;
			OnProgressChanged(originalProgress);
		}

		protected void OnProgressChanged(float newProgress)
		{
			Assert.IsTrue(newProgress >= 0 && newProgress <= 1);

            if (this == null)
            {
                // We have been destroyed
                return;
            }

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
            Logging.VerboseMethod(Tags.PROGRESS_BARS);
			EnableImages(true);
		}

		private void HideProgressBar()
		{
            Logging.VerboseMethod(Tags.PROGRESS_BARS);
			EnableImages(false);
		}

		private void EnableImages(bool enabled)
		{
			progressBarOutline.enabled = enabled;
			progressSoFar.enabled = enabled;
		}

		public void UpdateSize(float width, float height)
		{
			Vector2 size = new Vector2(width, height);
			progressBarOutline.rectTransform.sizeDelta = size;
			progressSoFar.rectTransform.sizeDelta = size;
			_outlineWidth = width;
		}
	}
}
