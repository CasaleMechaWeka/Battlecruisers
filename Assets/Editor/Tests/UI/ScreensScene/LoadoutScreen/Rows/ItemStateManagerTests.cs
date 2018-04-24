using System.Collections.Generic;
using System.Linq;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.UI.ScreensScene.LaodoutScreen.Rows
{
    public class ItemStateManagerTests
    {
        private IItemStateManager _stateManager;

        private List<IStatefulUIElement> _allItems, _cruiserItems, _buildingItems, _unitItems;

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _stateManager = new ItemStateManager();

            _cruiserItems = new List<IStatefulUIElement>()
            {
                Substitute.For<IStatefulUIElement>(),
                Substitute.For<IStatefulUIElement>()
            };
            AddItems(_cruiserItems, ItemType.Cruiser);

            _buildingItems = new List<IStatefulUIElement>()
            {
				Substitute.For<IStatefulUIElement>(),
				Substitute.For<IStatefulUIElement>()
            };
            AddItems(_buildingItems, ItemType.Building);

            _unitItems = new List<IStatefulUIElement>()
            {
				Substitute.For<IStatefulUIElement>(),
				Substitute.For<IStatefulUIElement>()
            };
            AddItems(_unitItems, ItemType.Unit);

            _allItems = new List<IStatefulUIElement>();
            _allItems.AddRange(_cruiserItems);
            _allItems.AddRange(_buildingItems);
            _allItems.AddRange(_unitItems);
        }

        private void AddItems(IList<IStatefulUIElement> itemsToAdd, ItemType itemType)
        {
            foreach (IStatefulUIElement item in itemsToAdd)
            {
                _stateManager.AddItem(item, itemType);
            }
        }

        [Test]
        public void AddItem_Duplicate_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => _stateManager.AddItem(_cruiserItems.First(), ItemType.Cruiser));
        }

        [Test]
        public void HandleDetailsManagerDismissed_MakesAllItemsDefault()
        {
            _stateManager.HandleDetailsManagerDismissed();
            ItemsReceivedGoToState(_allItems, UIState.Default);
        }

        [Test]
        public void HandleDetailsManagerComparing_MakesAllItemsDefault()
        {
            _stateManager.HandleDetailsManagerComparing();
            ItemsReceivedGoToState(_allItems, UIState.Default);
        }

        [Test]
        public void HandleDetailsManagerReadyToCompare_ComparingCruisers_HighlightsCruisers_DisablesOthers()
        {
            _stateManager.HandleDetailsManagerReadyToCompare(ItemType.Cruiser);

            ItemsReceivedGoToState(_cruiserItems, UIState.Highlighted);
            ItemsReceivedGoToState(_buildingItems, UIState.Disabled);
            ItemsReceivedGoToState(_unitItems, UIState.Disabled);
        }

        [Test]
        public void HandleDetailsManagerReadyToCompare_ComparingUnits_HighlightsUnits_DisablesOthers()
        {
            _stateManager.HandleDetailsManagerReadyToCompare(ItemType.Unit);

            ItemsReceivedGoToState(_unitItems, UIState.Highlighted);
            ItemsReceivedGoToState(_cruiserItems, UIState.Disabled);
            ItemsReceivedGoToState(_buildingItems, UIState.Disabled);
        }

        private void ItemsReceivedGoToState(IList<IStatefulUIElement> items, UIState state)
        {
			foreach (IStatefulUIElement item in items)
			{
				item.Received().GoToState(state);
			}
        }
    }
}
