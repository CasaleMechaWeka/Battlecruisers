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
            IPrefabFactory prefabFactory)
        {
            IBuildableWrapper<IUnit> unitPrefab = prefabFactory.GetUnitWrapperPrefab(Key);
            UnitButtonV2 unitButton = GetComponentInChildren<UnitButtonV2>(includeInactive: true);
            Assert.IsNotNull(unitButton);
            unitButton.Initialise(soundPlayer, itemDetailsManager, comparingFamilyTracker, unitPrefab, unitKeyName);
            return unitButton;
        }

        protected override bool IsUnlocked(IGameModel gameModel)
        {
            return gameModel.UnlockedUnits.Contains(Key);
        }

        protected override bool IsNew(IGameModel gameModel)
        {
            return gameModel.NewUnits.Items.Contains(Key);
        }

        protected override void MakeOld(IGameModel gameModel)
        {
            gameModel.NewUnits.RemoveItem(Key);
        }
    }
}