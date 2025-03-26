namespace BattleCruisers.UI.ScreensScene.ShopScreen
{
    public class RankData
    {
        public string RankImage { get; set; }
        public string RankNumber { get; set; }
        public string RankNameKeyBase { get; set; }

        public RankData(string rankImage, string rankNumber, string rankNameKeyBase)
        {
            RankImage = rankImage;
            RankNumber = rankNumber;
            RankNameKeyBase = rankNameKeyBase;
        }
    }
}