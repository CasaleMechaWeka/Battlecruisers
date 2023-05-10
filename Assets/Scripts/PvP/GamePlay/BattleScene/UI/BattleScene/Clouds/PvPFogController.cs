using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Clouds
{
    public class PvPFogController : MonoBehaviour
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