using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Utils.PlatformAbstractions.UI
{
    public class FillableImage : IFillableImage
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

        public FillableImage(Image platformImage)
        {
            Assert.IsNotNull(platformImage);
            _platformImage = platformImage;
        }
    }
}