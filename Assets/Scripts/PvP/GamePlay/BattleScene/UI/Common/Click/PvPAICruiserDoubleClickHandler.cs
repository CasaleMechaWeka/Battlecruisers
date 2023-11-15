using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.UserChosen;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.Click
{
    public class PvPAICruiserDoubleClickHandler : IPvPDoubleClickHandler<IPvPCruiser>
    {
        private readonly IPvPUserChosenTargetHelper _userChosenTargetHelper;

        public PvPAICruiserDoubleClickHandler(IPvPUserChosenTargetHelper userChosenTargetHelper)
        {
            Assert.IsNotNull(userChosenTargetHelper);
            _userChosenTargetHelper = userChosenTargetHelper;
        }

        public void OnDoubleClick(IPvPCruiser aiCruiser)
        {
            Assert.AreEqual(PvPFaction.Reds, aiCruiser.Faction);
            _userChosenTargetHelper.ToggleChosenTarget(aiCruiser);
        }
    }
}