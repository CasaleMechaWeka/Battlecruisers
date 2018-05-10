using System;
using BattleCruisers.Utils;

namespace BattleCruisers.Buildables.BuildProgress
{
    // FELIX  Test :)

    /// <summary>
    /// Forwards calculation to the appropriate calculator, depending on what
    /// build speed is set.
    /// </summary>
    public class CompositeCalculator : IBuildProgressCalculator, IBuildSpeedController
    {
        private readonly IBuildProgressCalculator _slowCalculator, _normalCalculator, _fastCalculator;
        private IBuildProgressCalculator _currentCalculator;

        private const BuildSpeed DEFAULT_BUILD_SPEED = BuildSpeed.Normal;

        public BuildSpeed BuildSpeed
        {
            set
            {
                _currentCalculator = ChooseCalculator(value);
            }
        }

        public CompositeCalculator(
            IBuildProgressCalculator slowCalculator, 
            IBuildProgressCalculator normalCalculator, 
            IBuildProgressCalculator fastCalculator)
        {
            Helper.AssertIsNotNull(slowCalculator, normalCalculator, fastCalculator);

            _slowCalculator = slowCalculator;
            _normalCalculator = normalCalculator;
            _fastCalculator = fastCalculator;

            BuildSpeed = DEFAULT_BUILD_SPEED;
        }

        public float CalculateBuildProgressInDroneS(IBuildable buildableUnderConstruction, float deltaTime)
        {
            return _currentCalculator.CalculateBuildProgressInDroneS(buildableUnderConstruction, deltaTime);
        }

        private IBuildProgressCalculator ChooseCalculator(BuildSpeed speed)
        {
            switch (speed)
            {
				case BuildSpeed.InfinitelySlow:
					return _slowCalculator;
                case BuildSpeed.Normal:
                    return _normalCalculator;
                case BuildSpeed.VeryFast:
                    return _fastCalculator;
                default:
                    throw new ArgumentException();
            }
        }
    }
}