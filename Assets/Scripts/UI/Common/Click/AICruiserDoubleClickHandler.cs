using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Common.Click
{
    public class AICruiserDoubleClickHandler : IDoubleClickHandler<ICruiser>
    {
        private readonly IUserChosenTargetHelper _userChosenTargetHelper;

        public AICruiserDoubleClickHandler(IUserChosenTargetHelper userChosenTargetHelper)
        {
            Assert.IsNotNull(userChosenTargetHelper);
            _userChosenTargetHelper = userChosenTargetHelper;
        }

        public void OnDoubleClick(ICruiser aiCruiser)
        {
            Assert.AreEqual(Faction.Reds, aiCruiser.Faction);
            _userChosenTargetHelper.ToggleChosenTarget(aiCruiser);
        }
    }
}