using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Turrets.AntiShip
{
    public class MortarMovingTargetTestGod : MortarTestGod
    {
        public List<Vector2> targetPatrolPoints;

        protected override List<Vector2> TargetPatrolPoints => targetPatrolPoints;
    }
}
