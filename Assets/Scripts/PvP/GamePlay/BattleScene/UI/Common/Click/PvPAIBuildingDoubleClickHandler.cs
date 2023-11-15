using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.UserChosen;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.Click
{
    public class PvPAIBuildingDoubleClickHandler : IPvPDoubleClickHandler<IPvPBuilding>
    {
        private readonly IPvPUserChosenTargetHelper _userChosenTargetHelper;

        public PvPAIBuildingDoubleClickHandler(IPvPUserChosenTargetHelper userChosenTargetHelper)
        {
            Assert.IsNotNull(userChosenTargetHelper);
            _userChosenTargetHelper = userChosenTargetHelper;
        }

        public void OnDoubleClick(IPvPBuilding aiBuliding)
        {
            Assert.AreEqual(PvPFaction.Reds, aiBuliding.Faction);
            _userChosenTargetHelper.ToggleChosenTarget(aiBuliding);
        }
    }
}