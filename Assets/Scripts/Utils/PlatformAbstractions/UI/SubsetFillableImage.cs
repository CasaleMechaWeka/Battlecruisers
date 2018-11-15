using UnityEngine.Assertions;

namespace BattleCruisers.Utils.PlatformAbstractions.UI
{
    /// <summary>
    /// FELIX
    /// </summary>
    /// FELIX  Test :D
    public class SubsetFillableImage : IFillableImage
    {
        private readonly IFillableImage _baseIamge;
        private readonly float _logicalFillAmount;
        
        // Giving a proportion of 0 will set this minimum proportion on the underlying image
        private readonly float _minProportion;
        
        // Giving a proportion of 1 will set this maximum proportion on the underlying image
        private readonly float _maxProportion;

        public bool IsVisible
        {
            get { return _baseIamge.IsVisible; }
            set { _baseIamge.IsVisible = value; }
        }

        public float FillAmount
        {
            // FELIX  Do reverse transform to get fill amount?
            get { return _logicalFillAmount; }
            set { _baseIamge.FillAmount = MapFillAmount(value); }
        }

        public SubsetFillableImage(IFillableImage baseImage, float minProportion, float maxProportion)
        {
            Assert.IsNotNull(baseImage);
            Assert.IsTrue(minProportion >= 0);
            Assert.IsTrue(maxProportion <= 1);
            Assert.IsTrue(minProportion < maxProportion);

            _baseIamge = baseImage;
            _minProportion = minProportion;
            _maxProportion = maxProportion;

            _logicalFillAmount = baseImage.FillAmount;
        }

        private float MapFillAmount(float fillAmount)
        {
            // FELIX  NEXT :D
            return 0;
        }
    }
}