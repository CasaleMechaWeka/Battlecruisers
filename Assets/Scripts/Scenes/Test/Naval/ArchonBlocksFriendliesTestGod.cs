using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using UnityEngine;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Naval
{
    public class ArchonBlocksFriendliesTestGod : TestGodBase
    {
        public ArchonBattleshipController archon;
        public AttackBoatController attackBoat;

        protected override List<GameObject> GetGameObjects()
        {
            BCUtils.Helper.AssertIsNotNull(archon, attackBoat);

            attackBoat.GameObject.SetActive(false);

            return new List<GameObject>()
            {
                archon.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            ICruiser cruiser = helper.CreateCruiser(Direction.Right, Faction.Blues);

            helper.InitialiseUnit(archon, parentCruiser: cruiser);
            archon.StartConstruction();
            Helper.SetupUnitForUnitMonitor(archon, cruiser);
            
            attackBoat.GameObject.SetActive(true);
            helper.InitialiseUnit(attackBoat, parentCruiser: cruiser);
            attackBoat.StartConstruction();
            Helper.SetupUnitForUnitMonitor(attackBoat, cruiser);
        }
    }
}
