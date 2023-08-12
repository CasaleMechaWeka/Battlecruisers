using BattleCruisers.UI.ScreensScene.ShopScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattleCruisers.UI.ScreensScene
{
    public class HeckleItemController : MonoBehaviour
    {
        public CanvasGroupButton clickingArea;
        public GameObject _ownedItemMark;
        public GameObject _clickedFeedback;
        private IHeckleData _heckleData;
        private ISingleSoundPlayer _soundPlayer;
        private HecklesContainer _hecklesContainer;
        public int _index;


        public void StaticInitialise(
            ISingleSoundPlayer soundPlayer,
            IHeckleData heckleData,
            HecklesContainer hecklesContainer,
            int index
            )
        {
            Helper.AssertIsNotNull(soundPlayer, /*prefabFactory, */heckleData,  clickingArea, _ownedItemMark, _clickedFeedback, hecklesContainer);

            _heckleData = heckleData;
            _soundPlayer = soundPlayer;
            _hecklesContainer = hecklesContainer;
            _index = index;

            _clickedFeedback.SetActive(false);

            _ownedItemMark.SetActive(_heckleData.IsOwned);
            clickingArea.Initialise(_soundPlayer, OnClicked);
        }
        private void OnClicked()
        {
            _clickedFeedback.SetActive(true);
            _hecklesContainer.heckleDataChanged.Invoke(this, new HeckleDataEventArgs
            {
                heckleData = _heckleData                
            });
        }
    }
}
