using BattleCruisers.UI.BattleScene.Manager;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers
{
    public class BackgroundController : MonoBehaviour, IPointerClickHandler
	{
        private IUIManager _uiManager;

        public void Initialise(IUIManager uIManager)
        {
            Assert.IsNotNull(uIManager);
            _uiManager = uIManager;
        }

		public void OnPointerClick(PointerEventData eventData)
		{
            _uiManager.HideItemDetails();
		}
	}
}
