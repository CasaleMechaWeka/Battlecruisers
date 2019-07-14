using System.Collections.Generic;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Sounds
{
    // Old plan???
    // 1. Add zooming, see if volume changes
    // 2. Add music, to have base volume
    // 3. Other sound effects?  Explosion???
    public class SoundsTestGod : MonoBehaviour
    {
        public List<Vector2> patrolPoints;

        void Start()
        {
            Helper helper = new Helper();
            TestAircraftController aircraft = FindObjectOfType<TestAircraftController>();
            aircraft.PatrolPoints = patrolPoints;
            helper.InitialiseUnit(aircraft);
            aircraft.StartConstruction();
        }
    }
}
