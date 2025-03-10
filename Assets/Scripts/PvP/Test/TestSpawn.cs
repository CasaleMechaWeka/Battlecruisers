using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using Unity.Netcode;
using UnityEngine;

public class TestSpawn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickRedBall()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            GameObject redball = GameObject.Instantiate(Resources.Load("RedBall")) as GameObject;
            redball.GetComponent<NetworkObject>().SpawnWithOwnership(SynchedServerData.Instance.playerAClientNetworkId.Value, true);
        }
    }

    public void OnClickBlueBall()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            GameObject blueball = GameObject.Instantiate(Resources.Load("BlueBall")) as GameObject;
            blueball.GetComponent<NetworkObject>().SpawnWithOwnership(SynchedServerData.Instance.playerBClientNetworkId.Value, true);
        }
    }
}
