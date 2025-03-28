using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Properties;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Items
{
    public class UnitItemContainerV2 : ItemContainer
    {
        public PrefabKeyName unitKeyName;

        private UnitKey _key;
        private UnitKey Key
        {
            get
            {
                if (_key == null)
                {
                    _key = StaticPrefabKeyHelper.GetPrefabKey<UnitKey>(unitKeyName);
                }
                return _key;
            }
        }

        protected override ItemButton InitialiseItemButton(
            IItemDetailsManager itemDetailsManager,
            IComparingItemFamilyTracker comparingFamilyTracker,
            IBroadcastingProperty<HullKey> selectedHull,
            ISingleSoundPlayer soundPlayer,
            GameModel gameModel)
        {
            IBuildableWrapper<IUnit> unitPrefab = PrefabFactory.GetUnitWrapperPrefab(Key);
            UnitButtonV2 unitButton = GetComponentInChildren<UnitButtonV2>(includeInactive: true);
            Assert.IsNotNull(unitButton);
            unitButton.Initialise(soundPlayer, itemDetailsManager, comparingFamilyTracker, unitPrefab, gameModel, Key);
            return unitButton;
        }

        protected override bool IsUnlocked(GameModel gameModel)
        {
            return gameModel.UnlockedUnits.Contains(Key);
        }

        protected override bool IsNew(GameModel gameModel)
        {
            return gameModel.NewUnits.Items.Contains(Key);
        }

        protected override void MakeOld(GameModel gameModel)
        {
            gameModel.NewUnits.RemoveItem(Key);
        }
    }
}