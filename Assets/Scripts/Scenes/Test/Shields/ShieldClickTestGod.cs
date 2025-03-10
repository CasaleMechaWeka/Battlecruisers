using BattleCruisers.Buildables.Buildings.Tactical.Shields;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Shields
{
    public class ShieldClickTestGod : TestGodBase
    {
        private ShieldGenerator _shield;

        protected override List<GameObject> GetGameObjects()
        {
            _shield = FindObjectOfType<ShieldGenerator>();

            return new List<GameObject>()
            {
                _shield.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            helper.InitialiseBuilding(_shield);
            _shield.StartConstruction();
        }
    }
}