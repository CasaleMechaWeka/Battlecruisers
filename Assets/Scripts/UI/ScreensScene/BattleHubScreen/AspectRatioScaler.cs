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

        private void Update()
        {
            float currentAspect = (float)Screen.width / Screen.height;
            float targetAspect = targetRatioX / targetRatioY;

            if (currentAspect < targetAspect)
            {
                float ratioDifference = (targetAspect - currentAspect) / (targetAspect - 1);
                float adjustedScale = Mathf.Clamp(1 + ratioDifference, 1, maxScale);

                transform.localScale = Vector3.one * adjustedScale;
            }
            else
            {
                transform.localScale = Vector3.one;
            }
        }
    }
}
