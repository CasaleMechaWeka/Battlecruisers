using BattleCruisers.Data;
using BattleCruisers.Data.Static;
using Unity.Netcode;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene
{
    public class PvPPlayerManager : NetworkBehaviour
    {
        NetworkVariable<NetworkString> prefabPathOfCruiser = new NetworkVariable<NetworkString>(readPerm: NetworkVariableReadPermission.Everyone, writePerm: NetworkVariableWritePermission.Owner);

        private void Start()
        {

        }
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (IsClient)
            {
                PrioritisedSoundKeys.SetSoundKeys(DataProvider.SettingsManager.AltDroneSounds);
            }
            else if (IsServer)
            {

            }
        }


        void Update()
        {
            // if (IsServer)
            // {
            //     if (prefabPathOfCruiser.Value.ToString().Split("=").Length == 2)
            //     {
            //         Debug.Log("Cruiser Name is " + prefabPathOfCruiser.Value.ToString().Split("=")[1]);
            //     }
            // }

        }
    }
}

