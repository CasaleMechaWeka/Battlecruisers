using UnityEngine;
using System.Collections;

namespace BattleCruisers.Buildables.BuildProgress
{
    public interface IBuildProgressCalculatorFactory
    {
        IBuildProgressCalculator CreatePlayerCruiserCalculator();
        IBuildProgressCalculator CreateAICruiserCalculator();
    }
}