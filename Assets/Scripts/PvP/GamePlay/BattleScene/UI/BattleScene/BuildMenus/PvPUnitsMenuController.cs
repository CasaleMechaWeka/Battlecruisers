using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.ClickHandlers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.BuildMenus
{
    public class PvPUnitsMenuController : PvPBuildablesMenuController<PvPUnitButtonController, IPvPUnit>
    {
        private IPvPSingleSoundPlayer _soundPlayer;
        private IPvPUnitClickHandler _unitClickHandler;
        private IPvPFactory _factory;

        public void Initialise(
            IPvPSingleSoundPlayer soundPlayer,
            IPvPUIManager uiManager,
            IPvPButtonVisibilityFilters buttonVisibilityFilters,
            IList<IPvPBuildableWrapper<IPvPUnit>> units,
            IPvPUnitClickHandler clickHandler)
        {
            // Need _unitClickHandler for abstract method called by base.Initialise().  Codesmell :P
            PvPHelper.AssertIsNotNull(soundPlayer, clickHandler);
            _soundPlayer = soundPlayer;
            _unitClickHandler = clickHandler;

            base.Initialise(uiManager, buttonVisibilityFilters, units);
        }

        protected override void InitialiseBuildableButton(PvPUnitButtonController button, IPvPBuildableWrapper<IPvPUnit> buildableWrapper)
        {
            button.Initialise(_soundPlayer, buildableWrapper, _shouldBeEnabledFilter, _unitClickHandler);
        }

        public override void OnPresenting(object activationParameter)
        {
            base.OnPresenting(activationParameter);

            _factory = activationParameter.Parse<IPvPFactory>();
            _factory.Destroyed += _factory_Destroyed;
        }

        private void _factory_Destroyed(object sender, EventArgs e)
        {
            _uiManager.HideCurrentlyShownMenu();
        }

        public override void OnDismissing()
        {
            base.OnDismissing();

            _factory.Destroyed -= _factory_Destroyed;
            _factory = null;
        }
    }
}
