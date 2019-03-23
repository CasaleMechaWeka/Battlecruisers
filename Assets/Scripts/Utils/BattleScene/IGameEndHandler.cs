namespace BattleCruisers.Utils.BattleScene
{
    public interface IGameEndHandler
    {
        void HandleCruiserDestroyed(bool wasPlayerVictory);
        void HandleGameEnd();
    }
}