using UnityEngine.Assertions;
using UnityEngine.UI;
using BattleCruisers.Utils.PlatformAbstractions.UI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.UI
{
    public class PvPFillableImage : IFillableImage
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