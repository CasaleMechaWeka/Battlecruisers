using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattleCruisers.UI.ScreensScene.ShopScreen
{
    public class RankData : IRankData
    {
        public string rankImage { get; set; }
        public string RankImage => rankImage;

        public string rankNumber { get; set; }
        public string RankNumber => rankNumber;

        public string rankNameKeyBase { get; set; }
        public string RankNameKeyBase => rankNameKeyBase;

        public RankData(string rankImage, string rankNumber, string rankNameKeyBase)
        {
            this.rankImage = rankImage;
            this.rankNumber = rankNumber;
            this.rankNameKeyBase = rankNameKeyBase;
        }
    }
}
