using UnityEngine.Assertions;

namespace BattleCruisers.Utils.PlatformAbstractions.UI
{
    /// <summary>
    /// Uses a subset of an underlying fillable image.  For example,
    /// with an underlying fillable image spanning 180*, and minimum
    /// and maximum proportions of 0.25 and 0.75 respectively, setting
    /// the FillAmount to:  Results in:
    ///                 0   => 0.25
    ///                 0.5 => 0.5
    ///                 1   => 0.75
    /// </summary>
    /// FELIX  Test :D
    public class SubsetFillableImage : IFillableImage
    {
        private readonly IFillableImage _baseIamge;
        private readonly float _range;
        
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
            get { return AdjustedToRaw(_baseIamge.FillAmount); }
            set { _baseIamge.FillAmount = RawToAdjusted(value); }
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

            _range = _maxProportion - _minProportion;
        }

        private float RawToAdjusted(float rawFillAmount)
        {
            float additionToMin = rawFillAmount * _range;
            return _minProportion + additionToMin;
        }

        private float AdjustedToRaw(float adjustedFillAmount)
        {
            float additionToMin = adjustedFillAmount - _minProportion;
            return additionToMin / _range;
        }
    }
}