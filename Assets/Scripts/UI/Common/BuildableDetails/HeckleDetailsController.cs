using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using BattleCruisers.UI.ScreensScene.ShopScreen;
using BattleCruisers.Utils.Properties;
using BattleCruisers.Data;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Data.Static;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public class HeckleDetailsController : MonoBehaviour
    {
        public GameObject heckleMessage;
        public Text heckleText;
        private ISettableBroadcastingProperty<HeckleData> _selectedItem;
        public IBroadcastingProperty<HeckleData> SelectedItem;

        public void Initialize()
        {
            _selectedItem = new SettableBroadcastingProperty<HeckleData>(initialValue: StaticData.Heckles[0]); // Heckle000 is default;
            SelectedItem = new BroadcastingProperty<HeckleData>(_selectedItem);
        }

        public void ShowHeckle(HeckleData heckleData)
        {
            _selectedItem.Value = heckleData;
            heckleText.text = LocTableCache.HecklesTable.GetString(heckleData.StringKeyBase);
            heckleMessage.GetComponent<RectTransform>().localScale = Vector3.zero;
            heckleMessage.GetComponent<RectTransform>().DOScale(Vector3.one, 0.2f);
            gameObject.SetActive(true);
        }

        public void HideDetails()
        {
            gameObject.SetActive(false);
        }
    }
}
