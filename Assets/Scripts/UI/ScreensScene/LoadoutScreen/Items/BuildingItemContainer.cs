using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using UnityCommon.Properties;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Items
{
    public class BuildingItemContainer : ItemContainer
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
            ISoundPlayer soundPlayer, 
            IPrefabFactory prefabFactory)
        {
            IBuildableWrapper<IBuilding> buildingPrefab = prefabFactory.GetBuildingWrapperPrefab(Key);
            BuildingButton buildingButton = GetComponentInChildren<BuildingButton>(includeInactive: true);
            Assert.IsNotNull(buildingButton);
            buildingButton.Initialise(soundPlayer, itemDetailsManager, comparingFamilyTracker, buildingPrefab);
            return buildingButton;
        }

        protected override bool IsUnlocked(IGameModel gameModel)
        {
            return gameModel.UnlockedBuildings.Contains(Key);
        }

        protected override bool IsNew(IGameModel gameModel)
        {
            return gameModel.NewBuildings.Items.Contains(Key);
        }

        protected override void MakeOld(IGameModel gameModel)
        {
            gameModel.NewBuildings.RemoveItem(Key);
        }
    }
}