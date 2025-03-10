using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    public class FogController : MonoBehaviour
    {
        public void Initialise(Color fogColor)
        {
            Image[] fogImages = GetComponentsInChildren<Image>();

            foreach (Image fogImage in fogImages)
            {
                fogImage.color = fogColor;
            }
        }
    }
}