using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.Common.Click;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.Click
{
    public class PvPAICruiserDoubleClickHandler : IDoubleClickHandler<IPvPCruiser>
    {
        private readonly IUserChosenTargetHelper _userChosenTargetHelper;

        public PvPAICruiserDoubleClickHandler(IUserChosenTargetHelper userChosenTargetHelper)
        {
            Assert.IsNotNull(userChosenTargetHelper);
            _userChosenTargetHelper = userChosenTargetHelper;
        }

        public void OnDoubleClick(IPvPCruiser aiCruiser)
        {
            Assert.AreEqual(Faction.Reds, aiCruiser.Faction);
            _userChosenTargetHelper.ToggleChosenTarget(aiCruiser);
        }
    }
}