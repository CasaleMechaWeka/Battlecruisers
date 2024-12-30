using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.ProfileScreen
{
    public class CaptainExoItem : MonoBehaviour
    {
        [SerializeField] private Sprite _captainExoImage;
        [SerializeField] private string _captainExoNameText;
        [SerializeField] private int _captainExoCostText;

        public void SetCaptainData(CaptainExo captainExoData)
        {
            // Assign the captainData values to the UI Item's UI elements
            // You can access the UI elements using GetComponentInChildren, Find, or public references set in the Inspector
            _captainExoImage = captainExoData.CaptainExoImage;
/*            _captainExoNameText = captainExoData.CaptainExoName;
            _captainExoCostText = captainExoData.CaptainExoCost;*/
        }
    }
}

