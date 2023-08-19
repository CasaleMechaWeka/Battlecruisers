using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using UnityEngine;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.ShopScreen;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Items
{
    public class HeckleButtonV2 : ItemButton
    {
        private RectTransform _selectedFeedback;
        public override IComparableItem Item => null;
        private IHeckleData _heckleData;

        public override void ShowDetails()
        {
          //  _itemDetailsManager.ShowDetails(null);
        }

        public void Initialise(
            ISingleSoundPlayer soundPlayer,
            IHeckleData heckleData,
            IItemDetailsManager itemDetailsManager,
            IComparingItemFamilyTracker comparingFamilyTracker)
        {
            base.Initialise(soundPlayer, itemDetailsManager, comparingFamilyTracker);
            _selectedFeedback = transform.FindNamedComponent<RectTransform>("SelectedFeedback");
            _heckleData = heckleData;
            _itemName.text = Mathf.Max(108, 217 * heckleData.Index).ToString().Substring(0, 3);
        }
    }
}
