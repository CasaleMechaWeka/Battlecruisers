using BattleCruisers.Buildables;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Naval
{
    public class AttackBoatAttackingCruiserTestGod : MonoBehaviour
    {
        void Start()
        {
            Helper helper = new Helper();

            // Setup fake cruiser
            TestTarget fakeCruiser = FindObjectOfType<TestTarget>();
            fakeCruiser.Initialise(Faction.Reds);

            // Setup attack boat
        }
    }
}