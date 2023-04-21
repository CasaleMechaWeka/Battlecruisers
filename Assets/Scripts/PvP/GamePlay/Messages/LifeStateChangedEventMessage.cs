using BattleCruisers.Network.Multiplay.Utils;
using BattleCruisers.Network.Multiplay.Gameplay.GameplayObjects;
using BattleCruisers.Network.Multiplay.Gameplay.GameplayObjects.Character;
using Unity.Netcode;



namespace BattleCruisers.Network.Multiplay.Gameplay.Messages
{
    public struct LifeStateChangedEventMessage : INetworkSerializeByMemcpy
    {
        public LifeState NewLifeState;
        public CharacterTypeEnum CharacterType;
        public FixedPlayerName CharacterName;
    }
}

