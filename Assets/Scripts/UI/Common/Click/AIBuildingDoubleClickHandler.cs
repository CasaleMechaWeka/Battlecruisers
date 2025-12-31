using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Common.Click
{
    public class AIBuildingDoubleClickHandler : IDoubleClickHandler<IBuilding>
    {
        private readonly IUserChosenTargetHelper _userChosenTargetHelper;

        public AIBuildingDoubleClickHandler(IUserChosenTargetHelper userChosenTargetHelper)
        {
            Assert.IsNotNull(userChosenTargetHelper);
            _userChosenTargetHelper = userChosenTargetHelper;
        }

        public void OnDoubleClick(IBuilding aiBuliding)
        {
            Assert.AreEqual(Faction.Reds, aiBuliding.Faction);
            _userChosenTargetHelper.ToggleChosenTarget(aiBuliding);
        }
    }
}