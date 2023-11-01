using BattleCruisers.UI.ScreensScene.ShopScreen;
using BattleCruisers.UI.ScreensScene;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.UI;
using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene
{
    public class VariantItemController : MonoBehaviour
    {
        public Image _parentImage;
        public Image _perkImage;
        public CanvasGroupButton clickingArea;
        public GameObject _ownedItemMark;
        public GameObject _clickedFeedback;
        private IVariantData _variantData;
        private ISingleSoundPlayer _soundPlayer;
        private VariantsContainer _variantsContainer;
        private Sprite _parentSprite;
        private Sprite _perkSprite;
        public int _index;

        public void StaticInitialise(
            ISingleSoundPlayer soundPlayer,
            Sprite spriteParent,
            Sprite spritePerk,
            IVariantData variantData,
            VariantsContainer variantsContainer,
            int index
            )
        {
            Helper.AssertIsNotNull(soundPlayer, variantData, spriteParent, spritePerk, clickingArea, _ownedItemMark, _clickedFeedback, variantsContainer);
            _variantData = variantData;
            _soundPlayer = soundPlayer;
            _variantsContainer = variantsContainer;
            _parentSprite = spriteParent;
            _perkSprite = spritePerk;
            _index = index;

            _parentImage.sprite = _parentSprite;
            _perkImage.sprite = _perkSprite;
            _clickedFeedback.SetActive(false);

            _ownedItemMark.SetActive(_variantData.IsOwned);
            clickingArea.Initialise(_soundPlayer, OnClicked);
        }

        public void OnClicked()
        {
            if (!_clickedFeedback.activeSelf)
            {
                _clickedFeedback.SetActive(true);
                //_variantContainer.variantDataChanged.Invoke(this, new BodykitDataEventArgs
                //{
                //    variantData = _variantData,
                //    variantImage = _variantSprite
                //});
            }
        }
    }
}
