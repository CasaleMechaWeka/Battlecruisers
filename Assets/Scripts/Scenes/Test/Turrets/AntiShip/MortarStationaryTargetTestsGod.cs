using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Turrets.AntiShip
{
    public class MortarStationaryTargetTestsGod : MortarTestGod
    {
        protected override List<Vector2> TargetPatrolPoints
        {
            get
            {
                // Aircraft is never stationary, so provide very cloase patorl points
                // to mimic a stationary target.
                return new List<Vector2>() { new Vector2(0, 0), new Vector2(0, -0.01f) };
            }
        }
    }
}
