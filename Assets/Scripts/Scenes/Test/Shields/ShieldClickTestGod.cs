using BattleCruisers.Buildables.Buildings.Tactical.Shields;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Shields
{
    public class ShieldClickTestGod : MonoBehaviour
    {
        void Start()
        {
            Helper helper = new Helper();

            ShieldGenerator shield = FindObjectOfType<ShieldGenerator>();
            helper.InitialiseBuilding(shield);
            shield.StartConstruction();
        }
    }
}