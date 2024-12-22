using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.ShopScreen;
using BattleCruisers.Utils.Properties;
using BattleCruisers.Data;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public class HeckleDetailsController : MonoBehaviour
    {
        public GameObject heckleMessage;
        public Text heckleText;
        private ISettableBroadcastingProperty<IHeckleData> _selectedItem;
        public IBroadcastingProperty<IHeckleData> SelectedItem;

        public void Initialize()
        {
            _selectedItem = new SettableBroadcastingProperty<IHeckleData>(initialValue: ApplicationModelProvider.ApplicationModel.DataProvider.GameModel.Heckles[0]); // Heckle000 is default;
            SelectedItem = new BroadcastingProperty<IHeckleData>(_selectedItem);
        }

        public void ShowHeckle(IHeckleData heckleData)
        {
            _selectedItem.Value = heckleData;
            heckleText.text = LandingSceneGod.Instance.hecklesStrings.GetString(heckleData.StringKeyBase);
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
