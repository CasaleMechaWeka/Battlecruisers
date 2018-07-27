using BattleCruisers.Buildables.Boost;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using System;

namespace BattleCruisers.Cruisers.Slots
{
    public class SlotBoostFeedback
    {
        private readonly ITextMesh _textMesh;
        private readonly IObservableCollection<IBoostProvider> _boostProviders;

        private int NumOfBoosters
        {
            set
            {
                switch (value)
                {
                    case 0:
                        _textMesh.SetActive(isActive: false);
                        break;

                    case 1:
                        _textMesh.SetActive(isActive: true);
                        _textMesh.Text = "+";
                        break;

                    case 2:
                        _textMesh.SetActive(isActive: true);
                        _textMesh.Text = "++";
                        break;

                    default:
                        throw new ArgumentException("Unsupported number of boosters: " + value);
                }
            }
        }

        public SlotBoostFeedback(ITextMesh textMesh, IObservableCollection<IBoostProvider> boostProviders)
        {
            Helper.AssertIsNotNull(textMesh, boostProviders);

            _textMesh = textMesh;
            _boostProviders = boostProviders;

            NumOfBoosters = 0;
            _boostProviders.Changed += _boostProviders_Changed;
        }

        private void _boostProviders_Changed(object sender, CollectionChangedEventArgs<IBoostProvider> e)
        {
            NumOfBoosters = _boostProviders.Items.Count;
        }
    }
}