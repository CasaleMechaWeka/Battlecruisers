using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils.Fetchers;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Buttons.Filters
{
    public class BuildingNameFilter : IBroadcastingFilter<IBuildable>, IBuildingPermitter
    {
        private readonly IPrefabFactory _prefabFactory;

        private IBuilding _permittedBuilding;
        public IPrefabKey PermittedBuilding
        {
            set
            {
                _permittedBuilding = value != null ? _prefabFactory.GetBuildingWrapperPrefab(value).Buildable : null;

                PotentialMatchChange?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler PotentialMatchChange;

        public BuildingNameFilter(IPrefabFactory prefabFactory)
        {
            Assert.IsNotNull(prefabFactory);
            _prefabFactory = prefabFactory;
        }

        /// <summary>
        /// Note:
        /// 1. Only one buildable can be permitted at a time
        /// 2. Only a building can be permitted (no units).  That is because currently
        ///     the tutorial does NOT involve creating any units.
        /// </summary>
        /// <returns><c>true</c>, if the given buildable name matches the permitted building name, <c>false</c> otherwise.</returns>
        public bool IsMatch(IBuildable buildable)
        {
            return
                _permittedBuilding != null
                && _permittedBuilding.Name == buildable.Name;
        }
    }
}
