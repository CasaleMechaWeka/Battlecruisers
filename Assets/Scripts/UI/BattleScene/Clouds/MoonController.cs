using BattleCruisers.UI.BattleScene.Clouds.Stats;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    public class MoonController : MonoBehaviour
    {
        public void Initialise(IMoonStats moonStats)
        {
            Image moon = GetComponent<Image>();
            Assert.IsNotNull(moon);

            if (!moonStats.ShowMoon)
            {
                transform.parent.gameObject.SetActive(false);
            }
            else
            {
                transform.parent.gameObject.SetActive(true);
                RectTransform rectTransform = (RectTransform)transform;
                rectTransform.localPosition = moonStats.MoonTransform.position;
                rectTransform.sizeDelta = moonStats.MoonTransform.sizeDelta;
                moon.color = moonStats.Color;
            }
        }
    }
}