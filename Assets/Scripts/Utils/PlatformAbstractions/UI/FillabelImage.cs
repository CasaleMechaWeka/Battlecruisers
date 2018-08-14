using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Utils.PlatformAbstractions.UI
{
    public class FillabelImage : IFillabelImage
    {
        private readonly Image _platformImage;

        public float FillAmount
        {
            get { return _platformImage.fillAmount; }
            set { _platformImage.fillAmount = value; }
        }

        public FillabelImage(Image platformImage)
        {
            Assert.IsNotNull(platformImage);
            _platformImage = platformImage;
        }
    }
}