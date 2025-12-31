using Unity.Netcode;

namespace BattleCruisers.Network.Multiplay.Gameplay.Messages
{
    public class DoorStateChangedEventMessage : INetworkSerializeByMemcpy
    {
        public bool IsDoorOpen;
    }
}

