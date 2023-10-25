using BattleCruisers.UI;
using BattleCruisers.UI.ScreensScene;
using BattleCruisers.UI.ScreensScene.ShopScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BodykitItemController : MonoBehaviour
{
    public Image _bodyKitImage;
    public CanvasGroupButton clickingArea;
    public GameObject _ownedItemMark;
    public GameObject _clickedFeedback;
    private IBodykitData _bodykitData;
    private ISingleSoundPlayer _soundPlayer;
    private BodykitsContainer _bodykitContainer;
    private Sprite _bodykitSprite;
    public int _index;
    

    public void StaticInitialise(
        ISingleSoundPlayer soundPlayer,
        Sprite spriteBodykit,
        IBodykitData bodykitData,
        BodykitsContainer bodykitContainer,
        int index
        )
    {
        Helper.AssertIsNotNull(soundPlayer, bodykitData, _bodyKitImage, clickingArea, _ownedItemMark, _clickedFeedback, bodykitContainer);
        _bodykitData = bodykitData;
        _soundPlayer = soundPlayer;
        _bodykitContainer = bodykitContainer;
        _bodykitSprite = spriteBodykit;
        _index = index;

        _bodyKitImage.sprite = _bodykitSprite;
        _clickedFeedback.SetActive(false);

        _ownedItemMark.SetActive(_bodykitData.IsOwned);
        clickingArea.Initialise(_soundPlayer, OnClicked);
    }

    public void OnClicked()
    {
        if(!_clickedFeedback.activeSelf)
        {
            _clickedFeedback.SetActive(true);
            _bodykitContainer.bodykitDataChanged.Invoke(this, new BodykitDataEventArgs
            {
                bodykitData = _bodykitData,
                bodykitImage = _bodykitSprite
            });
        }
    }
}
