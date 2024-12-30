using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class PvPVLSBarrelWrapper : PvPStaticBarrelWrapper
    {
        [SerializeField]
        private PvPBarrelController barrelController;

        protected override float DesiredAngleInDegrees => 90;

        protected override void InitialiseBarrelController(PvPBarrelController barrel, IPvPBarrelControllerArgs args)
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
