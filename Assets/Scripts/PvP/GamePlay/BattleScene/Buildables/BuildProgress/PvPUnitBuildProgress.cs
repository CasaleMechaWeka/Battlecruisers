using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress
{
    public class PvPUnitBuildProgress : IPvPUnitBuildProgress
    {
        private readonly string _unitName;
        private readonly IPvPBuildProgressFeedback _buildProgressFeedback;

        public PvPUnitBuildProgress(string unitName, IPvPBuildProgressFeedback buildProgressFeedback)
        {
            Assert.IsFalse(string.IsNullOrWhiteSpace(unitName));
            Assert.IsNotNull(buildProgressFeedback);

            _unitName = unitName;
            _buildProgressFeedback = buildProgressFeedback;
        }

        public void ShowBuildProgressIfNecessary(IPvPUnit unit, IPvPFactory factory)
        {
            if (unit != null
                && unit.Name == _unitName)
            {
                _buildProgressFeedback.ShowBuildProgress(unit, factory);
            }
            else
            {
                _buildProgressFeedback.HideBuildProgress();
            }
        }
    }
}