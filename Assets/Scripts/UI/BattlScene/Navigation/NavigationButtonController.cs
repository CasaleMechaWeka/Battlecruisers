using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public class NavigationButtonController : UIElement
    {
        public void Initialise(UnityAction onClickHandler)
        {
            base.Initialise();

            Button button = GetComponent<Button>();
            Assert.IsNotNull(button);
            button.onClick.AddListener(onClickHandler);
        }
    }
}
