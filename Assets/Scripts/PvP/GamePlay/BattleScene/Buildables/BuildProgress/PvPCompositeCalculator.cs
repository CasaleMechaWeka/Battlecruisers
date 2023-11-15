using System;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress
{
    /// <summary>
    /// Forwards calculation to the appropriate calculator, depending on what
    /// build speed is set.
    /// </summary>
    public class PvPCompositeCalculator : IPvPBuildProgressCalculator, IPvPBuildSpeedController
    {
        private readonly IPvPBuildProgressCalculator _slowCalculator, _normalCalculator, _fastCalculator;
        private IPvPBuildProgressCalculator _currentCalculator;

        private const PvPBuildSpeed DEFAULT_BUILD_SPEED = PvPBuildSpeed.Normal;

        public PvPBuildSpeed BuildSpeed
        {
            set
            {
                _currentCalculator = ChooseCalculator(value);
            }
        }

        public PvPCompositeCalculator(
            IPvPBuildProgressCalculator slowCalculator,
            IPvPBuildProgressCalculator normalCalculator,
            IPvPBuildProgressCalculator fastCalculator)
        {
            PvPHelper.AssertIsNotNull(slowCalculator, normalCalculator, fastCalculator);

            _slowCalculator = slowCalculator;
            _normalCalculator = normalCalculator;
            _fastCalculator = fastCalculator;

            BuildSpeed = DEFAULT_BUILD_SPEED;
        }

        public float CalculateBuildProgressInDroneS(IPvPBuildable buildableUnderConstruction, float deltaTime)
        {
            return _currentCalculator.CalculateBuildProgressInDroneS(buildableUnderConstruction, deltaTime);
        }

        private IPvPBuildProgressCalculator ChooseCalculator(PvPBuildSpeed speed)
        {
            switch (speed)
            {
                case PvPBuildSpeed.InfinitelySlow:
                    return _slowCalculator;
                case PvPBuildSpeed.Normal:
                    return _normalCalculator;
                case PvPBuildSpeed.VeryFast:
                    return _fastCalculator;
                default:
                    throw new ArgumentException();
            }
        }
    }
}