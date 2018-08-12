using UnityEngine;

namespace BattleCruisers.Utils.PlatformAbstractions
{
    public class TimeBC : ITime
    {
        public float TimeScale
        {
            get { return Time.timeScale; }
            set { Time.timeScale = value; }
        }
    }
}