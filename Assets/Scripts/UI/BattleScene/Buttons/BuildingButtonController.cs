using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Scenes.BattleScene;
using BattleCruisers.UI.BattleScene.Buttons.ClickHandlers;
using BattleCruisers.UI.Filters;
using BattleCruisers.UI.Sound.Players;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public class BuildingButtonController : BuildableButtonController, IPointerDownHandler, IEndDragHandler, IDragHandler
    {
		private IBuildableWrapper<IBuilding> _buildingWrapper;
        private IBuildingClickHandler _clickHandler;
        private Transform _clickAndDragIcon;
        private Vector3 _originalClickAndDragPosition;
        private BuildableClickAndDrag _buildableClickAndDrag;
        public void Initialise(
            ISingleSoundPlayer soundPlayer,
            IBuildableWrapper<IBuilding> buildingWrapper, 
            IBuildingClickHandler clickHandler,
            IBroadcastingFilter<IBuildable> shouldBeEnabledFilter)
		{
            base.Initialise(soundPlayer, buildingWrapper.Buildable, shouldBeEnabledFilter);
			
			_buildingWrapper = buildingWrapper;
            _clickHandler = clickHandler;
            _clickAndDragIcon = transform.Find("ClickAndDragIcon");
            _originalClickAndDragPosition = transform.position;
            Image clickAndDragIcon = _clickAndDragIcon.GetComponent<Image>();
            clickAndDragIcon.sprite = buildableImage.sprite;
            _buildableClickAndDrag = GameObject.Find("BuildableClickAndDrag").GetComponentInChildren<BuildableClickAndDrag>();
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
