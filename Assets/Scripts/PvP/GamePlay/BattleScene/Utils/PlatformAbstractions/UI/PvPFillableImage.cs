using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.UI
{
    public class PvPFillableImage : IPvPFillableImage
    {
        private readonly Image _platformImage;

        public float FillAmount
        {
            get { return _platformImage.fillAmount; }
            set { _platformImage.fillAmount = value; }
        }

        public bool IsVisible
        {
            get { return _platformImage.IsActive(); }
            set { _platformImage.gameObject.SetActive(value); }
        }

        public PvPFillableImage(Image platformImage)
        {
            Assert.IsNotNull(platformImage);
            _platformImage = platformImage;
        }
    }
}