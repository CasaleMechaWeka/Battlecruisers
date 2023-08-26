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
        private float previuosIndex;

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
            index = Mathf.RoundToInt(index);
            if (index != previuosIndex)
            {
                if (index <= .5f)
                    _buttons[0].OnClickedAction();
                else if (index == 1f)
                    _buttons[1].OnClickedAction();
                else if (index == 2f)
                    _buttons[2].OnClickedAction();
                else if (index == 3f)
                    _buttons[3].OnClickedAction();
                else if (index == 4f)
                    _buttons[4].OnClickedAction();
                else if (index == 5f)
                    _buttons[5].OnClickedAction();
                else if (index == 6f)
                    _buttons[6].OnClickedAction();
                else if (index == 7f)
                    _buttons[7].OnClickedAction();
                else if (index == 8f)
                    transform.FindNamedComponent<HeckleCategoryButton>("HeckleButton").OnClickedAction();
                previuosIndex = index;
            }
        }

    }
}