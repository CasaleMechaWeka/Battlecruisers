using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Aircraft
{
    public class GunshipTestGod : MonoBehaviour
    {
        void Start()
        {
            Helper helper = new Helper();

            GunshipController gunship = FindObjectOfType<GunshipController>();
            helper.InitialiseBuildable(gunship);
            gunship.StartConstruction();
        }
    }
}
