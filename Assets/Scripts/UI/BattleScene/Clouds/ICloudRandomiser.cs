namespace BattleCruisers.UI.BattleScene.Clouds
{
    public interface ICloudRandomiser
    {
        void RandomiseStartingPosition(ICloud leftCloud, ICloud rightCloud);
    }
}