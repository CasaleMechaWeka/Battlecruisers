using UnityEngine;

namespace BattleCruisers.UI.BattleScene
{
    [RequireComponent(typeof(RectTransform))]
    public class HUDPanelScaler : MonoBehaviour
    {
        [SerializeField]
        private float aspectRatioBreakpoint = 2.1f;

        [SerializeField]
        private float scaleChangeAmount = 0.2f;

        private RectTransform rectTransformThis;

        private void Awake()
        {
            rectTransformThis = GetComponent<RectTransform>();
            MakeBackgroundPanelFit();
        }

        public void MakeBackgroundPanelFit()
        {
            float screenAspect = (float)Screen.width / Screen.height;
            float scaleAmount = 1.0f;

            if (screenAspect < aspectRatioBreakpoint)
            {
                scaleAmount = Mathf.Lerp(1.0f, screenAspect / aspectRatioBreakpoint, Mathf.Abs(screenAspect - aspectRatioBreakpoint) / scaleChangeAmount);
            }

            rectTransformThis.localScale = new Vector2(scaleAmount, scaleAmount);
        }
    }
}
