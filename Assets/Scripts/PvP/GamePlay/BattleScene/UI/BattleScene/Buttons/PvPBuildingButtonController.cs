using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
// using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.ClickHandlers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons
{
    public class PvPBuildingButtonController : PvPBuildableButtonController, IPointerDownHandler, IEndDragHandler, IDragHandler
    {
        private IPvPBuildableWrapper<IPvPBuilding> _buildingWrapper;
        private IPvPBuildingClickHandler _clickHandler;
        private Transform _clickAndDragIcon;
        private Vector3 _originalClickAndDragPosition;
        private PvPBuildableClickAndDrag _buildableClickAndDrag;
      
        public void Initialise(
            IPvPSingleSoundPlayer soundPlayer,
            IPvPBuildableWrapper<IPvPBuilding> buildingWrapper,
            IPvPBuildingClickHandler clickHandler,
            IPvPBroadcastingFilter<IPvPBuildable> shouldBeEnabledFilter)
        {
            base.Initialise(soundPlayer, buildingWrapper.Buildable, shouldBeEnabledFilter);

            _buildingWrapper = buildingWrapper;
            _clickHandler = clickHandler;
      
            _clickAndDragIcon = transform.Find("ClickAndDragIcon");
            _originalClickAndDragPosition = transform.position;
            Image clickAndDragIcon = _clickAndDragIcon.GetComponent<Image>();
            clickAndDragIcon.sprite = buildableImage.sprite;
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
                    mousePosition.y = mousePosition.y + (Screen.height / 5);//move to just above the pointer - this enables better visability of icon and for pointer to gameobject interactions
                }
                else
                {
                    mousePosition.y = mousePosition.y + (Screen.height / 10);//move to just above the pointer - this enables better visability of icon and for pointer to gameobject interactions
                }

                _clickAndDragIcon.position = mousePosition;
            }
        }
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);            
            _clickHandler.HandleClick(IsMatch, _buildingWrapper);
        }

        protected override void HandleClick(bool isButtonEnabled)
        {
            //we are now controlling the selection via OnPointerDown
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
