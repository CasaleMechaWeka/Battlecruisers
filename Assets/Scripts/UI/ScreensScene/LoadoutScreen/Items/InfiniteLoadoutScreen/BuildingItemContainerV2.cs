using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
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
    public class BuildingItemContainerV2 : ItemContainer
    {
        public PrefabKeyName buildingKeyName;

        private BuildingKey _key;
        private BuildingKey Key
        {
            get
            {
                if (_key == null)
                {
                    _key = StaticPrefabKeyHelper.GetPrefabKey<BuildingKey>(buildingKeyName);
                }
                return _key;
            }
        }

        protected override ItemButton InitialiseItemButton(
            IItemDetailsManager itemDetailsManager,
            IComparingItemFamilyTracker comparingFamilyTracker,
            IBroadcastingProperty<HullKey> selectedHull,
            SingleSoundPlayer soundPlayer,
            GameModel gameModel)
        {
            IBuildableWrapper<IBuilding> buildingPrefab = PrefabFactory.GetBuildingWrapperPrefab(Key);
            BuildingButtonV2 buildingButton = GetComponentInChildren<BuildingButtonV2>(includeInactive: true);
            Assert.IsNotNull(buildingButton);
            buildingButton.Initialise(soundPlayer, itemDetailsManager, comparingFamilyTracker, buildingPrefab, gameModel, Key);
            return buildingButton;
        }

        protected override bool IsUnlocked(GameModel gameModel)
        {
            return gameModel.UnlockedBuildings.Contains(Key);
        }

        protected override bool IsNew(GameModel gameModel)
        {
            return gameModel.NewBuildings.Items.Contains(Key);
        }

        protected override void MakeOld(GameModel gameModel)
        {
            gameModel.NewBuildings.RemoveItem(Key);
        }

        public BuildingKey GetKey()
        {
            return Key;
        }
    }
}