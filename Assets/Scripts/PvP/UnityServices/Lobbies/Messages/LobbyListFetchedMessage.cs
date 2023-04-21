using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.UnityServices.Lobbies
{
    public struct LobbyListFetchedMessage
    {
        public readonly IReadOnlyList<LocalLobby> LocalLobbies;

        public LobbyListFetchedMessage(List<LocalLobby> localLobbies)
        {
            LocalLobbies = localLobbies;
        }
    }
}

