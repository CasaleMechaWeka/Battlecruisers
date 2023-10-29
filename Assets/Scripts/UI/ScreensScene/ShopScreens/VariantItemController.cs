using BattleCruisers.UI.ScreensScene.ShopScreen;
using BattleCruisers.UI.ScreensScene;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.UI;
using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VariantItemController : MonoBehaviour
{
    public Image _variantImage;
    public CanvasGroupButton clickingArea;
    public GameObject _ownedItemMark;
    public GameObject _clickedFeedback;
    //private IVariantData _variantData;
    private ISingleSoundPlayer _soundPlayer;
    private VariantsContainer _variantsContainer;
    private Sprite _variantSprite;
    public int _index;

    public void StaticInitialise(
        ISingleSoundPlayer soundPlayer,
        Sprite spriteVariant,
        //IVariantData variantData,
        VariantsContainer variantsContainer,
        int index
        )
    {
        Helper.AssertIsNotNull(soundPlayer, /*variantData,*/ _variantImage, clickingArea, _ownedItemMark, _clickedFeedback, variantsContainer);
        //_variantData = variantData;
        _soundPlayer = soundPlayer;
        _variantsContainer = variantsContainer;
        _variantSprite = spriteVariant;
        _index = index;

        _variantImage.sprite = _variantSprite;
        _clickedFeedback.SetActive(false);

        //_ownedItemMark.SetActive(_variantData.IsOwned);
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
