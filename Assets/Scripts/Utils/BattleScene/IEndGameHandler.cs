namespace BattleCruisers.Utils.BattleScene
{
    public interface IEndGameHandler
    {
        void HandleCruiserDestroyed(bool wasPlayerVictory);
        void HandleGameEnd();
    }
}