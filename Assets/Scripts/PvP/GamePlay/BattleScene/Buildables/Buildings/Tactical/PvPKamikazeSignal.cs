using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Tactical
{
    public class PvPKamikazeSignal : PvPBuilding
    {
        protected override void OnBuildableCompleted()
        {
            if (IsServer)
            {
                base.OnBuildableCompleted();

                GameObject[] aircraftGameObjects = GameObject.FindGameObjectsWithTag(GameObjectTags.AIRCRAFT);

                foreach (GameObject aircraftGameObject in aircraftGameObjects)
                {
                    PvPAircraftController aircraftController = aircraftGameObject.GetComponentInChildren<PvPAircraftController>();
                    Assert.IsNotNull(aircraftController);

                    if (aircraftController.BuildableState == PvPBuildableState.Completed
                        && !aircraftController.IsDestroyed)
                    {
                        aircraftController.Kamikaze(EnemyCruiser);
                    }
                }

                OnBuildableCompletedClientRpc();
            }
            else
            {
                OnBuildableCompleted_PvPClient();
            }
        }
    }
}
