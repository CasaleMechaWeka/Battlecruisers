using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.UI;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.Filters;
using BattleCruisers.UI.Sound.Players;
using System;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons
{
    public class PvPBuildingCategoryButton : PvPCanvasGroupButton, IBuildingCategoryButton, IBroadcastingFilter
    {
        private PvPUIManager _uiManager;
        private IBroadcastingFilter<BuildingCategory> _shouldBeEnabledFilter;
        private FilterToggler _filterToggler;

        public Image activeFeedback;
        protected override MaskableGraphic Graphic => activeFeedback;

        public event EventHandler PotentialMatchChange
        {
            add { _shouldBeEnabledFilter.PotentialMatchChange += value; }
            remove { _shouldBeEnabledFilter.PotentialMatchChange -= value; }
        }

        public BuildingCategory category;
        public BuildingCategory Category => category;


        public bool IsMatch => _shouldBeEnabledFilter.IsMatch(Category);
        public bool IsActiveFeedbackVisible { set { activeFeedback.enabled = value; } }

        public void Initialise(
            ISingleSoundPlayer soundPlayer,
            BuildingCategory expectedBuildingCategory,
            PvPUIManager uiManager,
            IBroadcastingFilter<BuildingCategory> shouldBeEnabledFilter)
        {
            base.Initialise(soundPlayer);

            PvPHelper.AssertIsNotNull(activeFeedback, uiManager, shouldBeEnabledFilter);
            Assert.AreEqual(Category, expectedBuildingCategory);

            _uiManager = uiManager;
            _shouldBeEnabledFilter = shouldBeEnabledFilter;

            _filterToggler = new FilterToggler(this, this);
        }

        private void OnDestroy()
        {
            Destroy(activeFeedback);
        }

        public void TriggerClick()
        {
            OnClicked();
        }

        protected override void OnClicked()
        {
            base.OnClicked();
            _uiManager.SelectBuildingGroup(Category);
        }
    }
}
