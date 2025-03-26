using BattleCruisers.UI.ScreensScene.ShopScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.UI;
using BattleCruisers.UI.ScreensScene.ProfileScreen;

namespace BattleCruisers.UI.ScreensScene
{
    public class VariantItemController : MonoBehaviour
    {
        public Image _parentImage;
        public Image _variantImage;
        public CanvasGroupButton clickingArea;
        public GameObject _ownedItemMark;
        public GameObject _clickedFeedback;
        public Image _clickedFeedbackVariantImage;
        private VariantData _variantData;
        private ISingleSoundPlayer _soundPlayer;
        private VariantsContainer _variantsContainer;
        private Sprite _parentSprite;
        private Sprite _variantSprite;
        public string _parentName;
        public int _index;
        private VariantPrefab _variant;

        public void StaticInitialise(
            ISingleSoundPlayer soundPlayer,
            Sprite spriteParent,
            Sprite spriteVariant,
            string parentName,
            VariantData variantData,
            VariantsContainer variantsContainer,
            VariantPrefab variant,
            int index,
            bool isOwned)
        {
            Helper.AssertIsNotNull(soundPlayer, variantData, spriteParent, spriteVariant, parentName, clickingArea, _ownedItemMark, _clickedFeedback, variantsContainer, variant);
            _variantData = variantData;
            _soundPlayer = soundPlayer;
            _variantsContainer = variantsContainer;
            _parentSprite = spriteParent;
            _variantSprite = spriteVariant;
            _parentName = parentName;
            _variant = variant;
            _index = index;
            _parentImage.sprite = _parentSprite;
            _variantImage.sprite = _variantSprite;
            _clickedFeedback.SetActive(false);
            _clickedFeedbackVariantImage.color = new Color(_clickedFeedbackVariantImage.color.r, _clickedFeedbackVariantImage.color.g, _clickedFeedbackVariantImage.color.b, 64f / 255);
            _ownedItemMark.SetActive(isOwned);
            clickingArea.Initialise(_soundPlayer, OnClicked);
        }

        public void OnClicked()
        {
            _variantsContainer.onVariantItemClick.Invoke(this, new VariantDataEventArgs { });
            if (!_clickedFeedback.activeSelf)
            {
                _clickedFeedback.SetActive(true);
                _clickedFeedbackVariantImage.color = new Color(_clickedFeedbackVariantImage.color.r, _clickedFeedbackVariantImage.color.g, _clickedFeedbackVariantImage.color.b, 1f);
                _variantsContainer.variantDataChanged.Invoke(this, new VariantDataEventArgs
                {
                    variantData = _variantData,
                    parentSprite = _parentSprite,
                    variantSprite = _variantSprite,
                    parentName = _parentName,
                    varint = _variant,
                });
            }
        }
    }
}
