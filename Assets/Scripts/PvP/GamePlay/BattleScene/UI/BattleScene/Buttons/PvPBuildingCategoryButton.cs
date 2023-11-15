using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons
{
    public class PvPBuildingCategoryButton : PvPCanvasGroupButton, IPvPBuildingCategoryButton, IPvPBroadcastingFilter
    {
        private IPvPUIManager _uiManager;
        private IPvPBroadcastingFilter<PvPBuildingCategory> _shouldBeEnabledFilter;
        private PvPFilterToggler _filterToggler;

        public Image activeFeedback;
        protected override MaskableGraphic Graphic => activeFeedback;

        public event EventHandler PotentialMatchChange
        {
            add { _shouldBeEnabledFilter.PotentialMatchChange += value; }
            remove { _shouldBeEnabledFilter.PotentialMatchChange -= value; }
        }

        public PvPBuildingCategory category;
        public PvPBuildingCategory Category => category;


        public bool IsMatch => _shouldBeEnabledFilter.IsMatch(Category);
        public bool IsActiveFeedbackVisible { set { activeFeedback.enabled = value; } }

        public void Initialise(
            IPvPSingleSoundPlayer soundPlayer,
            PvPBuildingCategory expectedBuildingCategory,
            IPvPUIManager uiManager,
            IPvPBroadcastingFilter<PvPBuildingCategory> shouldBeEnabledFilter)
        {
            base.Initialise(soundPlayer);

            PvPHelper.AssertIsNotNull(activeFeedback, uiManager, shouldBeEnabledFilter);
            Assert.AreEqual(Category, expectedBuildingCategory);

            _uiManager = uiManager;
            _shouldBeEnabledFilter = shouldBeEnabledFilter;

            _filterToggler = new PvPFilterToggler(this, this);
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
