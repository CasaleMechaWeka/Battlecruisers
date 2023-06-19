using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.BattleHubScreen
{
    [RequireComponent(typeof(RectTransform))]
    public class AspectRatioScaler : MonoBehaviour
    {
        [Header("Aspect Ratio Settings")]
        [Tooltip("The aspect ratio (width) that your game is designed for.")]
        [SerializeField]
        private float targetRatioX = 16;

        [Tooltip("The aspect ratio (height) that your game is designed for.")]
        [SerializeField]
        private float targetRatioY = 10;

        [Header("Scale Settings")]
        [Tooltip("The maximum scale of the object. The object's scale will increase as the screen's aspect ratio narrows, up to this maximum.")]
        [SerializeField, Range(0, 2)]
        private float maxScale = 1.5f;

        [Header("Position Settings")]
        [Tooltip("The amount the object's Y position should change as the screen's aspect ratio narrows.")]
        [SerializeField]
        private float yPosChange = 10f;

        private Vector3 originalPosition;

        private void Awake()
        {
            originalPosition = transform.position;
        }

        private void Update()
        {
            float currentAspect = (float)Screen.width / Screen.height;
            float targetAspect = targetRatioX / targetRatioY;

            if (currentAspect < targetAspect)
            {
                float ratioDifference = (targetAspect - currentAspect) / (targetAspect - 1);
                float adjustedScale = Mathf.Clamp(1 + ratioDifference, 1, maxScale);
                float adjustedYPos = originalPosition.y + yPosChange * ratioDifference;

                transform.localScale = Vector3.one * adjustedScale;
                transform.position = new Vector3(originalPosition.x, adjustedYPos, originalPosition.z);
            }
            else
            {
                transform.localScale = Vector3.one;
                transform.position = originalPosition;
            }
        }
    }
}
