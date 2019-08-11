namespace BattleCruisers.Utils.BattleScene.Pools
{
    public interface IPoolable<TArgs>
    {
        void Activate(TArgs initialisationArgs);
        void Deactivate();
    }
}