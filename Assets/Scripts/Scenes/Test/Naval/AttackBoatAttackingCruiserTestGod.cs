using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Naval
{
    public class AttackBoatAttackingCruiserTestGod : MonoBehaviour
    {
        void Start()
        {
            // Setup fake cruiser
            TestTarget fakeCruiser = FindObjectOfType<TestTarget>();
            fakeCruiser.Initialise(Faction.Reds);

            // Setup attack boat
            Helper helper = new Helper();
            AttackBoatController attackBoat = FindObjectOfType<AttackBoatController>();
            helper.InitialiseUnit(attackBoat, Faction.Blues);
            attackBoat.StartConstruction();
        }
    }
}