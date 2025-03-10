namespace BattleCruisers.Utils.BattleScene
{
    public interface IGameEndHandler
    {
        void HandleCruiserDestroyed(bool wasPlayerVictory);
        void HandleGameEnd();
        void HandleCruiserDestroyed(bool wasPlayerVictory, long destructionScore);
    }
}