using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.Fetchers;
using Mono.CompilerServices.SymbolWriter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene
{
    public class ShopCaptainButton : ElementWithClickSound
    {
        public GameObject _itemImage;
        public ClickedFeedBack _clickedFeedBack;
        public GameObject _itemName;

        private ShopItemDisplayer _shopItemDisplayer;
        private CaptainExoData _captain;

        private CanvasGroup _canvasGroup;
        protected override CanvasGroup CanvasGroup => _canvasGroup;

        public bool UpdateClickedFeedback
        {
            set
            {
                _clickedFeedBack.IsVisible = value;
            }
        }
        
        public void Initialise(
            ISingleSoundPlayer soundPlayer,
            IPrefabFactory prefabFactory,
            CaptainExoData captainExo,
            ShopItemDisplayer itemDisplayer)
        {
            base.Initialise(soundPlayer);
            _shopItemDisplayer = itemDisplayer;
            _captain = captainExo;
            Image itemImage = _itemImage.GetComponent<Image>();
            itemImage.sprite = captainExo.CaptainExoImage;
            UpdateClickedFeedback = false;
            OwnedFeedback feedback = GetComponentInChildren<OwnedFeedback>();
            if(captainExo.IsOwned == true)
            {
                feedback.gameObject.SetActive(true);
            }
            else
            {
                feedback.gameObject.SetActive(false);
            }
        }

        protected override void OnClicked()
        {
            base.OnClicked();
            UpdateClickedFeedback = true;
            _shopItemDisplayer.DisplayItem(_captain);
        }
    }

}
