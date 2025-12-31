using Unity.Netcode;
using BattleCruisers.Network.Multiplay.Infrastructure;

public class BCNetworkManager : NetworkManager, INetworkObject
{
    public void DestroyNetworkObject()
    {
        Shutdown();
        Destroy(gameObject);
    }
}
