using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class VLSBarrelWrapper : StaticBarrelWrapper
    {
        [SerializeField]
        private BarrelController barrelController;

        protected override float DesiredAngleInDegrees => 90;

        protected override void InitialiseBarrelController(BarrelController barrel, IBarrelControllerArgs args)
        {
            if (barrelController == null)
            {
                Debug.LogError("BarrelController is not assigned in the Inspector.");
                return;
            }

            barrelController.InitialiseAsync(args);
        }
    }
}
