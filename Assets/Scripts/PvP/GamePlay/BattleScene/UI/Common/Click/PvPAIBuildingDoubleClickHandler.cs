using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.Click
{
    public class PvPAIBuildingDoubleClickHandler : IPvPDoubleClickHandler<IPvPBuilding>
    {
        private readonly IUserChosenTargetHelper _userChosenTargetHelper;

        public PvPAIBuildingDoubleClickHandler(IUserChosenTargetHelper userChosenTargetHelper)
        {
            Assert.IsNotNull(userChosenTargetHelper);
            _userChosenTargetHelper = userChosenTargetHelper;
        }

        public void OnDoubleClick(IPvPBuilding aiBuliding)
        {
            Assert.AreEqual(Faction.Reds, aiBuliding.Faction);
            _userChosenTargetHelper.ToggleChosenTarget(aiBuliding);
        }
    }
}