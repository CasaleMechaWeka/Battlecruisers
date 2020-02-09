using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.BuildProgress
{
    public class UnitBuildProgress : IUnitBuildProgress
    {
        private readonly string _unitName;
        private readonly IBuildProgressFeedback _buildProgressFeedback;

        public UnitBuildProgress(string unitName, IBuildProgressFeedback buildProgressFeedback)
        {
            Assert.IsFalse(string.IsNullOrWhiteSpace(unitName));
            Assert.IsNotNull(buildProgressFeedback);

            _unitName = unitName;
            _buildProgressFeedback = buildProgressFeedback;
        }

        public void ShowBuildProgressIfNecessary(IUnit unit, IFactory factory)
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