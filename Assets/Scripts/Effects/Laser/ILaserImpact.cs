using UnityEngine;

namespace BattleCruisers.Effects.Laser
{
    public interface ILaserImpact
    {
        void Show(Vector3 position);
        void Hide();
    }
}