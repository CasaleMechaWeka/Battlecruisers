using System;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils.Fetchers;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    /// <summary>
    /// Deems a buildable button as enabled if the buildable is the currently permitted
    /// building.
    /// 
    /// Note:
    /// 1. Only one buildable can be permitted at a time
    /// 2. Only a building can be permitted (no units).  That is because currently
    ///     the tutorial does NOT involve creating any units.
    /// </summary>
    /// FELIX  Test :D
    public class TutorialDecider : IBuildingPermitter, IBuildableButtonActivenessDecider
    {
        private readonly IPrefabFactory _prefabFactory;

        private IBuilding _permittedBuilding;
        public IPrefabKey PermittedBuilding
        {
            set
            {
                _permittedBuilding = value != null ? _prefabFactory.GetBuildingWrapperPrefab(value).Buildable : null;

                if (PotentialActivenessChange != null)
                {
                    PotentialActivenessChange.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler PotentialActivenessChange;

        public TutorialDecider(IPrefabFactory prefabFactory)
        {
            Assert.IsNotNull(prefabFactory);
            _prefabFactory = prefabFactory;
        }

        public bool ShouldBeEnabled(IBuildable buildable)
        {
            return
                _permittedBuilding != null
                && _permittedBuilding.Name == buildable.Name;
        }
    }
}
