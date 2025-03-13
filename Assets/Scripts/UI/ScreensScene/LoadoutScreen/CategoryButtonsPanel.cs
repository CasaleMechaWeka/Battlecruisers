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
using System;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public class CategoryButtonsPanel : MonoBehaviour, IManagedDisposable
    {
        private ItemCategoryButton[] _buttons;
        public Slider changeCategorySlider;
        private float[] categoryThresholds = new float[]
        {
            1f, 2.1f, 3.1f, 4.1f, 5f, 5.8f, 6.7f, 7.7f, 8.3f
        };

        private int previousCategory;

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
            int category = Array.BinarySearch(categoryThresholds, index);
            if (category < 0)
                category = ~category;

            if (category != previousCategory)
            {
                previousCategory = category;
                if (category < _buttons.Length)
                {
                    if (_buttons[category].isActiveAndEnabled)
                        _buttons[category].OnClickedAction();
                }
                else if (category == 8)
                    transform.FindNamedComponent<HeckleCategoryButton>("HeckleButton").OnClickedAction();
            }
        }

    }
}