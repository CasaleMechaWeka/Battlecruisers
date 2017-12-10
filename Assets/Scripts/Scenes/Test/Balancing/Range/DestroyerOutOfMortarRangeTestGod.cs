using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Balancing.Range
{
    public class DestroyerOutOfMortarRangeTestGod : MonoBehaviour
    {
        void Start()
        {
            Helper helper = new Helper();

            // Initialise mortar
            IBuilding mortar = FindObjectOfType<TurretController>();
            helper.InitialiseBuilding(mortar, Faction.Reds, parentCruiserDirection: Direction.Left);
            mortar.StartConstruction();

            // Initialise destroyers
            IUnit[] destroyers = FindObjectsOfType<ShipController>();
            foreach (IUnit destroyer in destroyers)
            {
                helper.InitialiseUnit(destroyer, Faction.Blues, parentCruiserDirection: Direction.Right);
                destroyer.StartConstruction();
            }
        }
    }
}
