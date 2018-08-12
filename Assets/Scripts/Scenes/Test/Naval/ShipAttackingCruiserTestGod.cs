using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Naval
{
    public class ShipAttackingCruiserTestGod : MonoBehaviour
    {
        void Start()
        {
            // Setup fake cruiser
            TestTarget fakeCruiser = FindObjectOfType<TestTarget>();
            fakeCruiser.Initialise(Faction.Reds);

            // Setup ship
            Helper helper = new Helper();
            ShipController attackBoat = FindObjectOfType<ShipController>();
            helper.InitialiseUnit(attackBoat, Faction.Blues);
            attackBoat.StartConstruction();
        }
    }
}