using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Utils.Localisation;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene
{
    public interface IPvPPrefab
    {
        void StaticInitialise(ILocTable commonStrings);
    }
}