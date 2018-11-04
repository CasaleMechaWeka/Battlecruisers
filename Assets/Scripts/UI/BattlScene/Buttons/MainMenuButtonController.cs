using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public class MainMenuButtonController : MonoBehaviour, IPointerClickHandler
    {
        private IMainMenuManager _mainMenuManager;

        public void Initialise(IMainMenuManager mainMenuManager)
        {
            Assert.IsNotNull(mainMenuManager);
            _mainMenuManager = mainMenuManager;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _mainMenuManager.ShowMenu();
        }
    }
}