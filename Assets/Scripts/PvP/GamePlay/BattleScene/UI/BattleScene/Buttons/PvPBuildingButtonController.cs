using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.ClickHandlers;
using BattleCruisers.UI.Filters;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using BattleCruisers.UI.Sound.Players;


namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons
{
    public class PvPBuildingButtonController : PvPBuildableButtonController, IPointerDownHandler, IEndDragHandler, IDragHandler
    {
        private IPvPBuildableWrapper<IPvPBuilding> _buildingWrapper;
        private PvPBuildingClickHandler _clickHandler;
        private Transform _clickAndDragIcon;
        private Vector3 _originalClickAndDragPosition;
        private PvPBuildableClickAndDrag _buildableClickAndDrag;

        public void Initialise(
            SingleSoundPlayer soundPlayer,
            IPvPBuildableWrapper<IPvPBuilding> buildingWrapper,
            PvPBuildingClickHandler clickHandler,
            IBroadcastingFilter<IPvPBuildable> shouldBeEnabledFilter,
            bool flipClickAndDragIcon)
        {
            base.ApplyVariantIfExist(buildingWrapper.Buildable);
            base.Initialise(soundPlayer, buildingWrapper.Buildable, shouldBeEnabledFilter);

            _buildingWrapper = buildingWrapper;
            _clickHandler = clickHandler;

            _clickAndDragIcon = transform.Find("ClickAndDragIcon");
            _originalClickAndDragPosition = transform.position;
            Image clickAndDragIcon = _clickAndDragIcon.GetComponent<Image>();
            clickAndDragIcon.sprite = buildableImage.sprite;
            if (flipClickAndDragIcon)
                clickAndDragIcon.transform.localScale = new Vector3(-1f, 1f, 1f);
            _buildableClickAndDrag = GameObject.Find("BuildableClickAndDrag").GetComponentInChildren<PvPBuildableClickAndDrag>();
        }

        public void Update()
        {

        }


        public void OnEndDrag(PointerEventData eventData)
        {
            _clickAndDragIcon.gameObject.SetActive(false);
            _clickAndDragIcon.position = _originalClickAndDragPosition;
            _buildableClickAndDrag.ClickAndDraging = false;
        }


        public void OnDrag(PointerEventData data)
        {
            if (isSelected)
            {
                _buildableClickAndDrag.ClickAndDraging = true;
                _clickAndDragIcon.gameObject.SetActive(true);
                Vector3 mousePosition = Input.mousePosition;
                if (SystemInfo.deviceType == DeviceType.Handheld)
                {
                    mousePosition.y = mousePosition.y + (Screen.height / 20);
                }
                else
                {
                    mousePosition.y = mousePosition.y + (Screen.height / 21);
                }

                _clickAndDragIcon.position = mousePosition;
            }
        }
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            HandleClick(IsMatch);
        }

        protected override void HandleClick(bool isButtonEnabled)
        {
            _clickHandler.HandleClick(IsMatch, _buildingWrapper);
        }

        public override void HandleHover()
        {
            _clickHandler.HandleHover(_buildingWrapper);
        }

        public override void HandleHoverExit()
        {
            _clickHandler.HandleHoverExit();
        }
    }
}
