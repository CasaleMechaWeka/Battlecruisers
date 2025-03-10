using BattleCruisers.Data.Models;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using System.Collections.Generic;
using BattleCruisers.Utils.Properties;
using UnityEngine;
using UnityEngine.UI;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public class CategoryButtonsPanel : MonoBehaviour, IManagedDisposable
    {
        private ItemCategoryButton[] _buttons;
        public Slider changeCategorySlider;
        private float previousIndex;

        public void Initialise(
            IItemPanelsController itemPanels,
            IBroadcastingProperty<ItemFamily?> itemFamilyToCompare,
            ISingleSoundPlayer soundPlayer,
            IGameModel gameModel,
            IList<IItemButton> itemButtons,
            IComparingItemFamilyTracker itemFamilyTracker)
        {
            Helper.AssertIsNotNull(itemPanels, itemFamilyToCompare, soundPlayer, gameModel, itemButtons);

            _buttons = GetComponentsInChildren<ItemCategoryButton>(includeInactive: true);

            foreach (ItemCategoryButton button in _buttons)
            {
                button.Initialise(soundPlayer, itemPanels, itemFamilyToCompare, gameModel, itemButtons, itemFamilyTracker);
            }

            HeckleCategoryButton heckleButton = transform.FindNamedComponent<HeckleCategoryButton>("HeckleButton");
            Assert.IsNotNull(heckleButton);
            heckleButton.Initialise(soundPlayer, itemPanels, itemFamilyToCompare, itemFamilyTracker);
            changeCategorySlider.onValueChanged.AddListener(ChangeCategory);
        }

        public void DisposeManagedState()
        {
            foreach (ItemCategoryButton button in _buttons)
            {
                button.DisposeManagedState();
            }
        }

        public void ChangeCategory(float index)
        {
            if (index != previousIndex)
            {
                if (index < 1f && _buttons[0].isActiveAndEnabled)
                    _buttons[0].OnClickedAction();
                else if (index < 2.1f && _buttons[1].isActiveAndEnabled)
                    _buttons[1].OnClickedAction();
                else if (index < 3.1f && _buttons[2].isActiveAndEnabled)
                    _buttons[2].OnClickedAction();
                else if (index < 4.1f && _buttons[3].isActiveAndEnabled)
                    _buttons[3].OnClickedAction();
                else if (index < 5f && _buttons[4].isActiveAndEnabled)
                    _buttons[4].OnClickedAction();
                else if (index < 5.8f && _buttons[5].isActiveAndEnabled)
                    _buttons[5].OnClickedAction();
                else if (index < 6.7f && _buttons[6].isActiveAndEnabled)
                    _buttons[6].OnClickedAction();
                else if (index < 7.7f && _buttons[7].isActiveAndEnabled)
                    _buttons[7].OnClickedAction();
                else if (index < 9f)
                    transform.FindNamedComponent<HeckleCategoryButton>("HeckleButton").OnClickedAction();
                previousIndex = index;
            }
        }

    }
}