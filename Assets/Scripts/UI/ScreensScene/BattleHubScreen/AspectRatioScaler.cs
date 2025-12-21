using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.BattleHubScreen
{
    [RequireComponent(typeof(RectTransform))]
    [AddComponentMenu("BattleCruisers/UI/AspectRatioScaler")]
    public class AspectRatioScaler : MonoBehaviour
    {
        [Header("General Function:")]
        [TextArea]
        [SerializeField]
        private string info = "This script adjusts the scale of the attached GameObject based on the current aspect ratio of the screen.";

        private Vector2 originalAnchoredPosition;
        private RectTransform rectTransform;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            originalAnchoredPosition = rectTransform.anchoredPosition;
        }

        [Header("Aspect Ratio Settings")]
        [Tooltip("The width part of the aspect ratio that your game is optimized for. For an aspect ratio of 16:9, this would be 16.")]
        [SerializeField]
        private float targetRatioX = 16;

        [Tooltip("The height part of the aspect ratio that your game is optimized for. For an aspect ratio of 16:9, this would be 9.")]
        [SerializeField]
        private float targetRatioY = 9;

        [Header("Scale Settings")]
        [Tooltip("The minimum scale of the object when the screen's aspect ratio is the same as or narrower than 'Minimum Scale Ratio'. This scale is relative to the object's original size.")]
        [SerializeField, Range(-2, 2)]
        private float minScale = 0.5f;

        [Tooltip("The aspect ratio (width/height) at which the object reaches its minimum scale. When the screen's aspect ratio is the same as or narrower than this value, the object will have the 'Min Scale'.")]
        [SerializeField]
        private float minScaleRatio = 5f / 4f;

        [Header("Position Settings")]
        [Tooltip("The minimum position offset on the X axis when the screen's aspect ratio is the same as or narrower than 'Minimum Scale Ratio'.")]
        [SerializeField]
        private float minPositionOffsetX = 0f;

        [Tooltip("The minimum position offset on the Y axis when the screen's aspect ratio is the same as or narrower than 'Minimum Scale Ratio'.")]
        [SerializeField]
        private float minPositionOffsetY = 0f;

        private void Update()
        {
            float currentAspect = (float)Screen.width / Screen.height;
            float targetAspect = targetRatioX / targetRatioY;

            if (currentAspect <= targetAspect)
            {
                float ratioDifference = Mathf.Clamp((targetAspect - currentAspect) / (targetAspect - minScaleRatio), 0, 1);
                float adjustedScale = 1 + ratioDifference * (minScale - 1);

                transform.localScale = Vector3.one * adjustedScale;
            }
            else
            {
                transform.localScale = Vector3.one;
            }

            UpdatePosition();
        }

        /// <summary>
        /// Adjusts the anchored position of the RectTransform based on the current aspect ratio.
        /// Sets the position to the original position plus the calculated offset when the aspect ratio
        /// is narrower than the target ratio.
        /// </summary>
        public void UpdatePosition()
        {
            float currentAspect = (float)Screen.width / Screen.height;
            float targetAspect = targetRatioX / targetRatioY;

            if (currentAspect <= targetAspect)
            {
                float ratioDifference = Mathf.Clamp((targetAspect - currentAspect) / (targetAspect - minScaleRatio), 0, 1);
                float adjustedOffsetX = ratioDifference * minPositionOffsetX;
                float adjustedOffsetY = ratioDifference * minPositionOffsetY;

                rectTransform.anchoredPosition = originalAnchoredPosition + new Vector2(adjustedOffsetX, adjustedOffsetY);
            }
            else
            {
                rectTransform.anchoredPosition = originalAnchoredPosition;
            }
        }
    }
}
