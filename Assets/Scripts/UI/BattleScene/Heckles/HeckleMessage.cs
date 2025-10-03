using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Data.Static;

namespace BattleCruisers.UI.BattleScene.Heckles
{
    /// <summary>
    /// Displays a heckle message in a speech bubble UI element.
    /// Non-networked version for single-player use.
    /// </summary>
    public class HeckleMessage : MonoBehaviour
    {
        public Text message;
        public float hideTime = 5f;
        private RectTransform messageFrame;

        public void Initialise()
        {
            Helper.AssertIsNotNull(message);
            messageFrame = GetComponent<RectTransform>();
            Hide();
        }

        /// <summary>
        /// Shows a heckle message with the specified index.
        /// </summary>
        /// <param name="heckleIndex">Index of the heckle (0-279)</param>
        public void Show(int heckleIndex)
        {
            if (heckleIndex < 0 || heckleIndex >= StaticData.Heckles.Count)
            {
                Debug.LogWarning($"Invalid heckle index: {heckleIndex}");
                return;
            }

            message.text = LocTableCache.HecklesTable.GetString(StaticData.Heckles[heckleIndex].StringKeyBase);
            messageFrame.localScale = Vector3.zero;
            messageFrame.DOScale(Vector3.one * 1.5f, 0.2f);
            
            // Cancel any pending hide invokes
            CancelInvoke("Hide");
            Invoke("Hide", hideTime);
        }

        public void Hide()
        {
            messageFrame.DOScale(Vector3.zero, 0.2f);
        }
    }
}

